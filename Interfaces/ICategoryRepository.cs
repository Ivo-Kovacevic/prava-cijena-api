using api.DTOs.Category;
using api.Models;

namespace api.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetBySlugAsync(string slug);
    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(string slug, UpdateCategoryRequestDto categoryDto);
    Task<Category?> DeleteAsync(string slug);
}