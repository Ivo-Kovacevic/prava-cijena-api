using api.Dto.Product;

namespace api.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsByCategorySlugAsync(string categorySlug);
    Task<ProductDto> GetProductBySlugAsync(string productSlug);
    Task<IEnumerable<ProductWithSimilarityDto>> SearchProduct(string productName);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId);
    Task<ProductDto> GetProductByIdAsync(Guid categoryId, Guid productId);
    Task<ProductDto> CreateProductAsync(Guid categoryId, CreateProductRequestDto productRequestDto);
    Task<ProductDto> UpdateProductAsync(Guid categoryId, Guid productId, UpdateProductRequestDto productRequestDto);
    Task DeleteProductAsync(Guid categoryId, Guid productId);
}