
using Mango.Services.OrderAPI.Models.DTOs;

namespace Mango.Services.OrderAPI.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto?>> GetProductsAsync();

    }
}
