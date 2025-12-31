using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDto> CreateOrder(CartDto cartDto);
    }
}
