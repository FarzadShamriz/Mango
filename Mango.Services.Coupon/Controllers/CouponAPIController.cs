using AutoMapper;
using Mango.Services.Coupon.Data;
using Mango.Services.Coupon.Models;
using Mango.Services.Coupon.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Coupon.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ResponseDto _response;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Models.Coupon> objList = _context.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Models.Coupon obj = _context.Coupons.First(c => c.CouponId == id);
                _response.Result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Models.Coupon obj = _context.Coupons.First(c => c.CouponCode.ToUpper() == code.ToUpper());
                _response.IsSuccess = obj == null ? false : true;
                _response.Result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }



        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                var newCoupon = _mapper.Map<Models.Coupon>(couponDto);
                _context.Coupons.Add(newCoupon);
                _context.SaveChangesAsync();
                _response.Result = _mapper.Map<CouponDto>(newCoupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                var newCoupon = _mapper.Map<Models.Coupon>(couponDto);
                _context.Coupons.Update(newCoupon);
                _context.SaveChangesAsync();
                _response.Result = _mapper.Map<CouponDto>(newCoupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                var retCoupon = _context.Coupons.First(c => c.CouponId == id);
                _context.Coupons.Remove(retCoupon);
                _context.SaveChangesAsync();
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
