using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Dto.StoreLocation;
using PravaCijena.Api.Helpers;

namespace PravaCijena.Api.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductWithStoresNumber>> GetProductsByCategorySlugAsync(string categorySlug, QueryObject query);
    Task<ProductDto> GetProductBySlugAsync(string productSlug);
    Task<List<StoreWithPriceDto>> GetProductStoresBySlugAsync(string productSlug);
    Task<IEnumerable<ProductWithSimilarityDto>> SearchProduct(string productName);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId, QueryObject query);
    Task<ProductDto> GetProductByIdAsync(Guid categoryId, Guid productId);
    Task<ProductDto> CreateProductAsync(Guid categoryId, CreateProductRequestDto productRequestDto);
    Task<ProductDto> UpdateProductAsync(Guid categoryId, Guid productId, UpdateProductRequestDto productRequestDto);
    Task DeleteProductAsync(Guid categoryId, Guid productId);
}