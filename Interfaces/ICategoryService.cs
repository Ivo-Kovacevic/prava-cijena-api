using api.Dto.Category;

namespace api.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> GetCategoryBySlugWithSubcategoriesAsync(string categorySlug);
    Task<IEnumerable<CategoryDto>> GetRootCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto categoryRequestDto);
    Task<CategoryDto> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequestDto categoryRequestDto);
    Task DeleteCategoryAsync(Guid categoryId);
}