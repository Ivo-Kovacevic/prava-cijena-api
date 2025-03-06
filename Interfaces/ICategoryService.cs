using api.Dto.Category;

namespace api.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto categoryRequestDto);
    Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto categoryRequestDto);
    Task DeleteCategoryAsync(Guid id);
    Task<bool> CategoryExists(Guid id);
}