using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
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
