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
    }
}
