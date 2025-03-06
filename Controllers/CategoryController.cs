using api.Dto.Category;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IEnumerable<CategoryDto>> Index()
    {
        return await _categoryService.GetCategoriesAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Show(Guid id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category is null)
        {
            return NotFound($"Category with id '{id}' not found.");
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Store(CreateCategoryRequestDto categoryRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (categoryRequestDto.ParentCategoryId.HasValue)
        {
            var parentCategoryExists = await _categoryService.CategoryExists(categoryRequestDto.ParentCategoryId.Value);
            if (!parentCategoryExists)
            {
                return NotFound($"Parent category with id '{categoryRequestDto.ParentCategoryId}' not found.");
            }
        }

        var createdCategory = await _categoryService.CreateCategoryAsync(categoryRequestDto);

        return CreatedAtAction(nameof(Show), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryRequestDto categoryRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (categoryRequestDto.ParentCategoryId.HasValue)
        {
            var parentCategoryExists = await _categoryService.CategoryExists(categoryRequestDto.ParentCategoryId.Value);
            if (!parentCategoryExists)
            {
                return NotFound($"Parent category with id '{categoryRequestDto.ParentCategoryId}' not found.");
            }
        }

        var categoryExists = await _categoryService.CategoryExists(id);
        if (!categoryExists)
        {
            return NotFound($"Category with id '{id}' not found.");
        }

        var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryRequestDto);

        return Ok(updatedCategory);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Destroy(Guid id)
    {
        var categoryExists = await _categoryService.CategoryExists(id);
        if (!categoryExists)
        {
            return NotFound($"Category with id '{id}' not found.");
        }

        await _categoryService.DeleteCategoryAsync(id);

        return NoContent();
    }
}