using api.DTOs;
using api.Models;

namespace api.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetProductsByCategoryAsync(string categorySlug);
    Task<Product?> GetBySlugAsync(string productSlug);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(int productId, Product product);
    Task<Product?> DeleteAsync(int productId);
}