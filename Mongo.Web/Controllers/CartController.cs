using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Name = cartDto.CartHeader.Name;
            cart.CartHeader.Email = cartDto.CartHeader.Email;

            var responseOrder = await _orderService.CreateOrder(cart);

            if (responseOrder != null && responseOrder.IsSuccess)
            {
                OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString((responseOrder.Result)));
                TempData["success"] = $"Success";
            }

            return View();
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);

            if (response != null && response.IsSuccess)
            {
                CartDto cartDto =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<CartDto>(
                        Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }

        public async Task<IActionResult> RemoveCart(int cartId)
        {
            ResponseDto response = await _cartService.RemoveCartAsync(cartId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Success";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = $"Error:{response.Message}";
            return RedirectToAction(nameof(CartIndex));
        }

        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            cartDto.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;

            ResponseDto response = await _cartService.EmailCart(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will be processed and send shortly.";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = $"Error:{response.Message}";
            return RedirectToAction(nameof(CartIndex));
        }

        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto response = await _cartService.ApplyCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Success";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = $"Error:{response.Message}";
            return RedirectToAction(nameof(CartIndex));
        }

        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDto response = await _cartService.ApplyCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Success";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = $"Error:{response.Message}";
            return RedirectToAction(nameof(CartIndex));
        }
    }
}
