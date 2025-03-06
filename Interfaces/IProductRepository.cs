using api.DTOs;
using api.Models;

namespace api.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId);
    Task<ProductDto?> GetProductByIdAsync(Guid productId);
    Task<ProductDto> CreateAsync(Product product);
    Task<ProductDto?> UpdateAsync(Guid productId, Product product);
    Task<bool> DeleteAsync(Guid productId);
}