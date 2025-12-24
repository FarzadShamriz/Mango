using Mongo.Web.Models;

namespace Mongo.Web.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductAsync(int id);
        Task<ResponseDto> GetAllProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto product);
        Task<ResponseDto?> UpdateProductAsync(ProductDto product);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
