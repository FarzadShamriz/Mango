using Microsoft.AspNetCore.Mvc;
using Mongo.Web.Models;
using Mongo.Web.Services.IServices;

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
