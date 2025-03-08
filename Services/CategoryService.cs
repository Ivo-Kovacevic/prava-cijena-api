using api.Dto.Category;
using api.Exceptions;
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
        var categories = await _categoryRepo.GetAllAsync();

        return categories.Select(c => c.ToCategoryDto());
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);

        if (category == null)
        {
            throw new NotFoundException($"Category with ID '{id}' not found.");
        }

        return category.ToCategoryDto();
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto categoryRequestDto)
    {
        var category = categoryRequestDto.CategoryFromCreateRequestDto();
        category = await _categoryRepo.CreateAsync(category);

        return category.ToCategoryDto();
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto categoryRequestDto)
    {
        var category = categoryRequestDto.CategoryFromUpdateRequestDto(id);
        category = await _categoryRepo.UpdateAsync(id, category);

        return category.ToCategoryDto();
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