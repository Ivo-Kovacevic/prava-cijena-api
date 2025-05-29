using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IProductService
{
    Task<Pagination> GetProductsByCategorySlugAsync(string categorySlug, string? userId, QueryObject query);

    Task<ProductDto> GetProductBySlugAsync(string productSlug);
    Task<List<StoreWithPriceDto>> GetProductStoresBySlugAsync(string productSlug);
    Task<IEnumerable<ProductDto>> SearchProduct(string searchTerm, int page, int limit);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId, QueryObject query);
    Task<ProductDto> GetProductByIdAsync(Guid categoryId, Guid productId);
    Task<ProductDto> CreateProductAsync(Guid categoryId, CreateProductRequestDto productRequestDto);
    Task<ProductDto> UpdateProductAsync(Guid categoryId, Guid productId, UpdateProductRequestDto productRequestDto);
    Task DeleteProductAsync(Guid categoryId, Guid productId);
}