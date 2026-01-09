using AutoMapper;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.IServices;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTOs;
using Mango.Services.OrderAPI.RabbitMQSender;
using Mango.Services.OrderAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IProductService _productService;
        private readonly RabbitMQOrderMessageSender _messageBus;
        private readonly IConfiguration _configuration;

        public OrderAPIController(AppDbContext db,
            IProductService productService, IMapper mapper, IConfiguration configuration
            , RabbitMQOrderMessageSender messageBus)
        {
            _db = db;
            _messageBus = messageBus;
            this._response = new ResponseDto();
            _productService = productService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

                OrderHeader orderCreated = _mapper.Map<OrderHeader>(orderHeaderDto);
                await _db.OrderHeaders.AddAsync(orderCreated);
                await _db.SaveChangesAsync();
                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;

                _response.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequest)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequest.ApprovedUrl,
                    CancelUrl = stripeRequest.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    Discounts = new List<SessionDiscountOptions>()
                };

                if (stripeRequest.orderHeader.Dicsount != null && stripeRequest.orderHeader.Dicsount > 0)
                {
                    options.Discounts.Add(new SessionDiscountOptions
                    {
                        Coupon = stripeRequest.orderHeader.CouponCode
                    });
                }

                foreach (var item in stripeRequest.orderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // 20.99 -> 2099
                            Currency = "cad",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductName,
                            },
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequest.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _db.OrderHeaders.First(o => o.OrderHeaderId == stripeRequest.orderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                await _db.SaveChangesAsync();
                _response.Result = stripeRequest;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(o => o.OrderHeaderId == orderHeaderId);
                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    //then payment was successful
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = SD.Status_Approved;
                    _db.SaveChanges();
                    RewardDto rewardDto = new RewardDto
                    {
                        UserId = orderHeader.UserId,
                        RewardsActity = Convert.ToInt32(orderHeader.OrderTotal), //1 point for every $1 spent
                        OrderId = orderHeader.OrderHeaderId
                    };
                    string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreateTopic");
                    _messageBus.SendMessage(rewardDto, topicName);

                    _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [Authorize]
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<ResponseDto> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == orderId);
                if (orderHeader != null)
                {
                    if (newStatus == SD.Status_Cancelled)
                    {
                        //we will give refund
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId
                        };

                        var service = new RefundService();
                        Refund refund = service.Create(options);
                    }
                    orderHeader.Status = newStatus;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
            }
            return _response;
        }

        
        [HttpGet("GetOrders")]
        public ResponseDto Get(string? userId = "")
        {
            try
            {
                IEnumerable<OrderHeader> orderHeaders;
                if (User.IsInRole(SD.RoleAdmin))
                {
                    orderHeaders = _db.OrderHeaders.
                        Include(o=>o.OrderDetails).
                        OrderByDescending(o=>o.OrderHeaderId).ToList();
                }
                else
                {
                    orderHeaders = _db.OrderHeaders.Where(o => o.UserId == userId).
                        Include(o=>o.OrderDetails).
                        OrderByDescending(o=>o.OrderHeaderId).ToList();
                }
                IEnumerable<OrderHeaderDto> orderHeaderDtos = _mapper.Map<IEnumerable<OrderHeaderDto>>(orderHeaders);
                _response.Result = orderHeaderDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [Authorize]
        [HttpGet("GetOrder/{orderId:int}")]
        public async Task<ResponseDto> Get(int orderId)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.Include(o=>o.OrderDetails).First(o => o.OrderHeaderId == orderId);
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(orderHeader);
                _response.Result = orderHeaderDto;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }


    }
}
