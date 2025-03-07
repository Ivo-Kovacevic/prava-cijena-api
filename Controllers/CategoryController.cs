using api.Dto.Category;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> Index()
    {
        var categoriesDto = await _categoryService.GetCategoriesAsync();
        return Ok(categoriesDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> Show(Guid id)
    {
        var categoryDto = await _categoryService.GetCategoryByIdAsync(id);
        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Store(CreateCategoryRequestDto categoryRequestDto)
    {
        var categoryDto = await _categoryService.CreateCategoryAsync(categoryRequestDto);
        return CreatedAtAction(nameof(Show), new { id = categoryDto.Id }, categoryDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> Update(Guid id, UpdateCategoryRequestDto categoryRequestDto)
    {
        var categoryDto = await _categoryService.UpdateCategoryAsync(id, categoryRequestDto);
        return Ok(categoryDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Destroy(Guid id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }
}