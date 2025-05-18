using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, QueryObject query);
    Task<Product?> GetProductBySlugAsync(string productSlug);
    Task<List<Product>> GetProductsBySlugsBatchAsync(IEnumerable<string> productSlugs);
    Task<Product?> GetProductByBarcodeAsync(string productBarcode);
    Task<List<Product>> GetProductsByBarcodesBatchAsync(IEnumerable<string> barcodes);
    Task<IEnumerable<ProductWithSimilarityDto>> Search(string searchTerm);
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<Product> CreateAsync(Product product);
    Task CreateProductsBatchAsync(List<Product> products);
    Task<Product> UpdateAsync(Product existingProduct);
    Task UpdateLowestPriceAsync(Guid productId, decimal price);
    Task UpdateLowestPricesBatchAsync(List<ProductPreview> productPreviews);
    Task DeleteAsync(Product existingProduct);
}