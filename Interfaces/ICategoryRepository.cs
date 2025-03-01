using api.DTOs.Category;
using api.Models;

namespace api.Interfaces;

public interface ICategoryRepository
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<Category?> GetBySlugAsync(string slug);
    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(int id, Category categoryDto);
    Task<Category?> DeleteAsync(int id);
}