using api.DTOs.Category;
using api.Models;

namespace api.Interfaces;

public interface ICategoryRepository
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<CategoryDto> CreateAsync(Category category);
    Task<CategoryDto?> UpdateAsync(Guid id, Category category);
    Task<CategoryDto?> DeleteAsync(Guid id);
    Task<bool> CategoryExists(Guid id);
}