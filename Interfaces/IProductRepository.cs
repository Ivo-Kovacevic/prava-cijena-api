using api.Models;

namespace api.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetBySlugAsync(string slug);
    // Task<Product> CreateAsync(Product category);
    // Task<Product?> UpdateAsync(string slug, UpdateProductRequestDto categoryDto);
    // Task<Product?> DeleteAsync(string slug);
}