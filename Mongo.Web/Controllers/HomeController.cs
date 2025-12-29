using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = new();
            ResponseDto response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(response.Result.ToString());
                TempData["success"] = response?.Message;
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            var responseModel = await _productService.GetProductByIdAsync(productId);
            if(responseModel != null && responseModel.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<ProductDto>(responseModel.Result.ToString());
                return View(model);
            }

            TempData["error"] = "Product Not Found!!!";
            return RedirectToAction(nameof(Index));

        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetails = new()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };

            List<CartDetailsDto> cartDetailsDtos = new() { cartDetails };

            cartDto.CartDetails = cartDetailsDtos;

            var addToCartResponse = await _cartService.UpsertCartAsync(cartDto);
            if (addToCartResponse != null && addToCartResponse.IsSuccess)
            {
                //var model = JsonConvert.DeserializeObject<CartDto>(addToCartResponse.Result.ToString());
                //return View(model);
                TempData["success"] = "Success";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Product Not Found!!!";
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
