using Mongo.Web.Models;
using Mongo.Web.Services.IServices;
using Mongo.Web.Utilities;

namespace Mongo.Web.Services
{
    public class ProductService : IProductService
    {

        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto product)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = $"{SD.ProductAPIBase}/api/Product",
                Data = product
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{SD.ProductAPIBase}/api/Product/{id}"
            });
        }

        public async Task<ResponseDto> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.ProductAPIBase}/api/Product"
            });
        }

        public async Task<ResponseDto?> GetProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.ProductAPIBase}/api/Product/{id}"
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.ProductAPIBase}/api/Product/{id}"
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto product)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Url = $"{SD.ProductAPIBase}/api/Product",
                Data = product
            });
        }
    }
}
