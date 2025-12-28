using Mango.Services.ShoppingCartAPI.IServices;
using Mango.Services.ShoppingCartAPI.Models.DTOs;
using Newtonsoft.Json;


namespace Mango.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //public async Task<IEnumerable<ProductDto>> GetProductsAsync(int id)
        //{
        //    var client = _httpClientFactory.CreateClient("Product");
        //    var response = await client.GetAsync($"/api/product/{id}");
        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    var deserializedResponse = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
        //    if(deserializedResponse != null || deserializedResponse.IsSuccess)
        //    {
        //        return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(deserializedResponse.Result));
        //    }
        //    return Enumerable.Empty<ProductDto>();
        //}

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");
            var responseContent = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<ResponseDto>(responseContent);
            if(deserializedResponse != null || deserializedResponse.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(deserializedResponse.Result));
            }
            return new List<ProductDto>();
        }
    }
}
