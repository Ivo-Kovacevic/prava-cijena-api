using PravaCijena.Api.Dto.Category;
using PravaCijena.Api.Exceptions;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Mappers;

namespace PravaCijena.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepo = categoryRepository;
    }

    /*
     * SLUG SERVICE
     */
    public async Task<CategoryDto> GetCategoryBySlugWithSubcategoriesAsync(string categorySlug)
    {
        var category = await _categoryRepo.GetBySlugWithFiltersAsync(categorySlug);
        if (category == null)
        {
            throw new NotFoundException($"Category '{categorySlug}' not found.");
        }

        return category.ToCategoryDto();
    }

    /*
     * ID SERVICE
     */
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _categoryRepo.GetAllCategoriesAsync();

        return categories.Select(c => c.ToCategoryDto());
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId)
    {
        var category = await _categoryRepo.GetByIdAsync(categoryId);
        if (category == null)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        return category.ToCategoryDto();
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto categoryRequestDto)
    {
        var category = categoryRequestDto.CategoryFromCreateRequestDto();
        category = await _categoryRepo.CreateAsync(category);

        return category.ToCategoryDto();
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequestDto categoryRequestDto)
    {
        var existingCategory = await _categoryRepo.GetByIdAsync(categoryId);
        if (existingCategory == null)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        existingCategory.CategoryFromUpdateRequestDto(categoryRequestDto);
        existingCategory = await _categoryRepo.UpdateAsync(existingCategory);

        return existingCategory.ToCategoryDto();
    }

    public async Task DeleteCategoryAsync(Guid categoryId)
    {
        var existingCategory = await _categoryRepo.GetByIdAsync(categoryId);
        if (existingCategory == null)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        await _categoryRepo.DeleteAsync(existingCategory);
    }
}