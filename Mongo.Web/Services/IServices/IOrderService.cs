using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDto> CreateOrder(CartDto cartDto);
        Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequest);
        Task<ResponseDto> ValidateStripeSession(int orderHeaderId);
    }
}
