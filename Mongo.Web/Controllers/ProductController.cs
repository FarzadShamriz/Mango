using Microsoft.AspNetCore.Mvc;
using Mongo.Web.Models;
using Mongo.Web.Services;
using Mongo.Web.Services.IServices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Mongo.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
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

        public async Task<IActionResult> ProductCreate()
        {
            ProductDto model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? cModel = new();
                ResponseDto response = await _productService.CreateProductAsync(productDto);

                if (response != null && response.IsSuccess)
                {
                    cModel = JsonConvert.DeserializeObject<ResponseDto>(response.Result.ToString());
                    TempData["success"] = response?.Message;
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(ProductIndex));
            }

            return View();
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            ProductDto model = new();
            var response = await _productService.GetProductByIdAsync(productId);
            if (response.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
                TempData["success"] = response?.Message;
                return View(model);
            }

            return NotFound($"Product #{productId} Not Found!!!");
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto product)
        {
            var resMsg = string.Empty;
            var response = await _productService.DeleteProductAsync(product.ProductId);

            if (response.IsSuccess)
            {
                TempData["success"] = response?.Message;
            }
            else
            {
                TempData["error"] = response?.Message;
                resMsg = response?.Message;
            }

            return RedirectToAction(nameof(ProductIndex));
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ProductDto model = new();
            var response = await _productService.GetProductByIdAsync(productId);
            if (response.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
                TempData["success"] = response?.Message;
                return View(model);
            }

            return NotFound($"Product #{productId} Not Found!!!");
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto product)
        {
            var resMsg = string.Empty;
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProductAsync(product);

                if (response.IsSuccess)
                {
                    ProductDto eModel = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
                    TempData["success"] = response?.Message;
                }
                else
                {
                    TempData["error"] = response?.Message;
                    resMsg = response?.Message;
                }
                return RedirectToAction(nameof(ProductIndex));

            }

            return NotFound(resMsg);
        }

    }
}
