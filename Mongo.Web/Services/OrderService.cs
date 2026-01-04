using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utilities;

namespace Mango.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateOrder(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = $"{SD.OrderAPIBase}/api/order/CreateOrder",
                Data = cartDto
            });
        }

        public async Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequest)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = $"{SD.OrderAPIBase}/api/order/CreateStripeSession",
                Data = stripeRequest
            });   
        }

        public async Task<ResponseDto> ValidateStripeSession(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = $"{SD.OrderAPIBase}/api/order/ValidateStripeSession",
                Data = orderHeaderId
            });
        }
        

        public async Task<ResponseDto> GetAllOrders(string? userId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.OrderAPIBase}/api/order/GetOrders?userId={userId}"
            });
        }

        public async Task<ResponseDto> GetOrder(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.OrderAPIBase}/api/order/GetOrder/{orderHeaderId}"
            });
        }

        public async Task<ResponseDto> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = $"{SD.OrderAPIBase}/api/order/UpdateOrderStatus/{orderId}",
                Data = newStatus
            });
        }
        
    }
}
