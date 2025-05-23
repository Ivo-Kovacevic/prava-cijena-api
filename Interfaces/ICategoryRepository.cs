using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetBySlugWithFiltersAsync(string categorySlug);
    Task<Category?> GetBySlugAsync(string categorySlug);
    Task<List<Category>> GetAllCategoriesAsync();
    Task<List<Category>> GetAllCategoriesWithSubcategoriesAsync();
    Task<Category?> GetByIdAsync(Guid categoryId);
    Task<Category> CreateAsync(Category category);
    Task<Category> UpdateAsync(Category existingCategory);
    Task DeleteAsync(Category existingCategory);
    Task<bool> CategoryExists(Guid categoryId);
}