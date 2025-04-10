using PravaCijena.Api.Dto.Category;

namespace PravaCijena.Api.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> GetCategoryBySlugWithSubcategoriesAsync(string categorySlug);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto categoryRequestDto);
    Task<CategoryDto> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequestDto categoryRequestDto);
    Task DeleteCategoryAsync(Guid categoryId);
}