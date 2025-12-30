using Mango.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserIdAsync(string userId);
        Task<ResponseDto> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto> RemoveCartAsync(int cartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
        Task<ResponseDto> EmailCart(CartDto cartDto);
    }
}
