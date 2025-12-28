using Mango.Web.Models;
using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string code);
        Task<ResponseDto> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDto coupon);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto coupon);
        Task<ResponseDto?> DeleteouponAsync(int id);

    }
}
