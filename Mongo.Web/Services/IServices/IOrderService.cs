using Mango.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDto> CreateOrder(CartDto cartDto);
        Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequest);
        Task<ResponseDto> ValidateStripeSession(int orderHeaderId);
        Task<ResponseDto> GetAllOrders(string? userId);
        Task<ResponseDto> GetOrder(int orderHeaderId);
        Task<ResponseDto> UpdateOrderStatus(int orderId, string newStatus);
    }
}
