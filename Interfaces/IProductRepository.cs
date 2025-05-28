using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, QueryObject query);

    Task<IEnumerable<ProductWithMetadata>> GetPageProductsByCategoryIdAsync(Guid categoryId, string? userId,
        QueryObject query);

    Task<Product?> GetProductBySlugAsync(string productSlug);
    Task<List<StoreWithPriceDto>> GetProductStoresBySlugsAsync(string productSlug);
    Task<List<string>> GetAllSlugsAsync();
    Task<List<string>> GetAllBarcodesAsync();
    Task<Product?> GetProductByBarcodeAsync(string productBarcode);
    Task<List<Product>> GetProductsByBarcodesBatchAsync(IEnumerable<string> barcodes);
    Task<List<Product>> Search(string searchTerm, int page, int limit);
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product existingProduct);
    Task UpdateLowestPriceAsync(Guid productId, decimal price);
    Task DeleteAsync(Product existingProduct);
    Task BulkCreateAsync(List<Product> products);
    Task BulkUpdateAsync(List<Product> products);
}