using AutoMapper;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.IServices;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTOs;
using Mango.Services.OrderAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public OrderAPIController(AppDbContext db, IMapper mapper, IProductService productService)
        {
            _db = db;
            _mapper = mapper;
            _productService = productService;
            this._response = new ResponseDto();
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

                if(stripeRequest.orderHeader.Dicsount != null && stripeRequest.orderHeader.Dicsount > 0)
                {
                    options.Discounts.Add(new SessionDiscountOptions
                    {
                        Coupon = stripeRequest.orderHeader.CouponCode
                    });
                }

                foreach(var item in stripeRequest.orderHeader.OrderDetails)
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

    }
}
