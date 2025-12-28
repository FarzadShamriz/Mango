
using Mango.Services.ShoppingCartAPI.Models.DTOs;

namespace Mango.Services.ShoppingCartAPI.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto?>> GetProductsAsync(int id);

    }
}
