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
        return await _categoryRepo.GetAllAsync();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var categoryDto = await _categoryRepo.GetByIdAsync(id);

        if (categoryDto == null)
        {
            throw new NotFoundException($"Category with ID '{id}' not found.");
        }

        return categoryDto;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto categoryRequestDto)
    {
        var category = categoryRequestDto.CategoryFromCreateRequestDto();
        return await _categoryRepo.CreateAsync(category);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto categoryRequestDto)
    {
        var categoryDto = await _categoryRepo.GetByIdAsync(id);

        if (categoryDto == null)
        {
            throw new NotFoundException($"Category with ID '{id}' not found.");
        }

        var category = categoryRequestDto.CategoryFromUpdateRequestDto();
        return await _categoryRepo.UpdateAsync(id, category);
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var categoryDto = await _categoryRepo.GetByIdAsync(id);

        if (categoryDto == null)
        {
            throw new NotFoundException($"Category with ID '{id}' not found.");
        }

        await _categoryRepo.DeleteAsync(id);
    }

    public async Task<bool> CategoryExists(Guid id)
    {
        return await _categoryRepo.CategoryExists(id);
    }
}