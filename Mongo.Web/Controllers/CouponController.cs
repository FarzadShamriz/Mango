using Mango.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Mongo.Web.Models;
using Mongo.Web.Services.IServices;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mongo.Web.Controllers
{
    public class CouponController : Controller
    {

        private ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();
            ResponseDto response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(response.Result.ToString());
                TempData["success"] = response?.Message;
            }
            else
            {
                TempData["error"] = response?.Message;
            }

                return View(list);
        }

        public async Task<IActionResult> CouponCreate()
        {
            CouponDto model = new();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                CouponDto? cModel = new();
                ResponseDto response = await _couponService.CreateCouponAsync(model);

                if (response != null && response.IsSuccess)
                {
                    cModel = JsonConvert.DeserializeObject<CouponDto>(response.Result.ToString());
                    TempData["success"] = response?.Message;
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(CouponIndex));
            }

            return View();
        }

        public async Task<IActionResult> CouponDelete(int id)
        {
            CouponDto? model = new();
            ResponseDto response = await _couponService.GetCouponByIdAsync(id);

            if (response != null && response.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<CouponDto>(response.Result.ToString());
                TempData["success"] = response?.Message;
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound($"Coupon #{id} Not Found!!!");
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto model)
        {
            CouponDto? responseModel = new();
            ResponseDto response = await _couponService.DeleteouponAsync(id:model.CouponId);

            if (response != null && response.IsSuccess)
            {
                //responseModel = JsonConvert.DeserializeObject<CouponDto>(response.Result.ToString());
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound(response.Message);
        }
    }
}
