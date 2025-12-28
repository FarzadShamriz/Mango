using Mango.Services.ShoppingCartAPI.IServices;
using Mango.Services.ShoppingCartAPI.Models.DTOs;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        public Task<IEnumerable<ProductDto?>> GetProductsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
