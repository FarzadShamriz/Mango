using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mongo.Web.Models;
using Mongo.Web.Services.IServices;
using Mongo.Web.Utilities;
using Newtonsoft.Json;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            ResponseDto responseDto = await _authService.Login(model);

            if(responseDto!= null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                TempData["success"] = "Loged in successfully";
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("CustomError", responseDto.Message);
            TempData["error"] = "Error!!!";
            return View(model);
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


        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {

            ResponseDto result = await _authService.Register(registrationRequestDto);
            ResponseDto assignRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDto.Role))
                {
                    registrationRequestDto.Role = SD.RoleCustomer;
                }

                assignRole = await _authService.AssignRole(registrationRequestDto);

                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() {Text = SD.RoleAdmin,Value = SD.RoleAdmin},
                new SelectListItem() {Text = SD.RoleCustomer,Value = SD.RoleCustomer}
            };

            ViewBag.RoleList = roleList;

            return View(registrationRequestDto);
        }


        [HttpGet]
        public IActionResult Logout()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }
    }
}
