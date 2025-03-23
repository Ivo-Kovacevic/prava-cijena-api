using api.Models;

namespace api.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllRootCategoriesAsync();
    Task<Category?> GetByIdAsync(Guid categoryId);
    Task<Category?> GetBySlugWithSubcategoriesAsync(string categorySlug);
    Task<Category> CreateAsync(Category category);
    Task<Category> UpdateAsync(Category existingCategory);
    Task DeleteAsync(Category existingCategory);
    Task<bool> CategoryExists(Guid categoryId);
}