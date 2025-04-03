using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Dto.Category;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /*
     * SLUG ENDPOINTS
     * These endpoints are for public access because they provide readable URLs
     */
    [HttpGet("{categorySlug}")]
    public async Task<ActionResult<CategoryDto>> ShowBySlug(string categorySlug)
    {
        var categoryDto = await _categoryService.GetCategoryBySlugWithSubcategoriesAsync(categorySlug);
        return Ok(categoryDto);
    }

    /*
     * ID ENDPOINTS
     * These endpoints are for internal use
     */
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> Index()
    {
        var categoriesDto = await _categoryService.GetRootCategoriesAsync();
        return Ok(categoriesDto);
    }

    [HttpGet("{categoryId:guid}")]
    public async Task<ActionResult<CategoryDto>> Show(Guid categoryId)
    {
        var categoryDto = await _categoryService.GetCategoryByIdAsync(categoryId);
        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Store(CreateCategoryRequestDto categoryRequestDto)
    {
        var categoryDto = await _categoryService.CreateCategoryAsync(categoryRequestDto);
        return CreatedAtAction(nameof(Show), new { categoryId = categoryDto.Id }, categoryDto);
    }

    [HttpPatch("{categoryId:guid}")]
    public async Task<ActionResult<CategoryDto>> Update(Guid categoryId, UpdateCategoryRequestDto categoryRequestDto)
    {
        var categoryDto = await _categoryService.UpdateCategoryAsync(categoryId, categoryRequestDto);
        return Ok(categoryDto);
    }

    [HttpDelete("{categoryId:guid}")]
    public async Task<IActionResult> Destroy(Guid categoryId)
    {
        await _categoryService.DeleteCategoryAsync(categoryId);
        return NoContent();
    }
}