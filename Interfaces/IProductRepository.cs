using api.Models;

namespace api.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
    Task<Product?> GetProductBySlugAsync(string productSlug);
    Task<IEnumerable<Product>> Search(string searchTerm);
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product existingProduct);
    Task DeleteAsync(Product existingProduct);
}