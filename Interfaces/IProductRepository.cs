using api.DTOs;
using api.Models;

namespace api.Interfaces;

public interface IProductRepository
{
    Task<List<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId);
    Task<ProductDto?> GetProductByIdAsync(Guid productId);
    Task<ProductDto> CreateAsync(Product product);
    Task<ProductDto?> UpdateAsync(Guid productId, Product product);
    Task<ProductDto?> DeleteAsync(Guid productId);
}