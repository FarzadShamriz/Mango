using Mango.Web.Models;
using Mongo.Web.Models;
using Mongo.Web.Services.IServices;
using Mongo.Web.Utilities;

namespace Mongo.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto coupon)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = Utilities.SD.ApiType.POST,
                Url = $"{SD.CouponAPIBase}/api/Coupon",
                Data = coupon
            });
        }

        public async Task<ResponseDto?> DeleteouponAsync(int id)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = Utilities.SD.ApiType.DELETE,
                Url = $"{SD.CouponAPIBase}/api/Coupon/{id}"
            });
        }

        public async Task<ResponseDto> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = Utilities.SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/coupon"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string code)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = Utilities.SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/Coupon/GetByCode/{code}"
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = Utilities.SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/Coupon/{id}"
            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto coupon)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = Utilities.SD.ApiType.PUT,
                Url = $"{SD.CouponAPIBase}/api/Coupon",
                Data = coupon
            });
        }
    }
}
