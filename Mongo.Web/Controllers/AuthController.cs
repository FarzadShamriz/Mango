using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mongo.Web.Models;
using Mongo.Web.Services.IServices;
using Mongo.Web.Utilities;

namespace Mongo.Web.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }


        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() {
                    Text = SD.RoleAdmin,
                    Value = SD.RoleAdmin
                },
                new SelectListItem() {
                    Text = SD.RoleCustomer,
                    Value = SD.RoleCustomer
                }
            };

            ViewBag.RoleList = roleList;

            return View();
        }


        [HttpGet]
        public IActionResult Logout()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }
    }
}
