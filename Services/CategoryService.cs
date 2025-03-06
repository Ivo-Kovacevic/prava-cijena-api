using api.Dto.Category;
using api.Interfaces;
using api.Mappers;

namespace api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepo = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        return await _categoryRepo.GetAllAsync();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        return await _categoryRepo.GetByIdAsync(id);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto categoryRequestDto)
    {
        var category = categoryRequestDto.CategoryFromCreateRequestDto();

        return await _categoryRepo.CreateAsync(category);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto categoryRequestDto)
    {
        var category = categoryRequestDto.CategoryFromUpdateRequestDto();
        var updatedCategory = await _categoryRepo.UpdateAsync(id, category);

        return updatedCategory;
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        await _categoryRepo.DeleteAsync(id);
    }

    public async Task<bool> CategoryExists(Guid id)
    {
        return await _categoryRepo.CategoryExists(id);
    }
}