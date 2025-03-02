using api.DTOs.Category;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepo = categoryRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<CategoryDto>> GetAll()
    {
        return await _categoryRepo.GetAllAsync();
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var category = await _categoryRepo.GetBySlugAsync(slug);
        if (category is null)
        {
            return NotFound($"Category with slug '{slug}' not found.");
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequestDto categoryRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (categoryRequestDto.ParentCategoryId.HasValue)
        {
            var parentCategory = await _categoryRepo.GetByIdAsync(categoryRequestDto.ParentCategoryId.Value);
            if (parentCategory == null)
            {
                return NotFound($"Parent category with id '{categoryRequestDto.ParentCategoryId}' not found.");
            }
        }

        var category = categoryRequestDto.CategoryFromRequestDto();
        var createdCategory = await _categoryRepo.CreateAsync(category);
            
        return CreatedAtAction(nameof(Get), new { slug = category.Slug }, createdCategory);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryRequestDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        if (categoryDto.ParentCategoryId.HasValue)
        {
            var parentCategory = await _categoryRepo.GetByIdAsync(categoryDto.ParentCategoryId.Value);
            if (parentCategory == null)
            {
                return NotFound($"Parent category with id '{categoryDto.ParentCategoryId}' not found.");
            }
        }
        
        var category = categoryDto.UpdateCategoryFromDto();

        var updatedCategory = await _categoryRepo.UpdateAsync(id, category);
        if (updatedCategory == null)
        {
            return NotFound($"Category with id '{id}' not found.");
        }

        return Ok(updatedCategory);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryRepo.DeleteAsync(id);
        if (category == null)
        {
            return NotFound($"Category with id '{id}' not found.");
        }

        return NoContent();
    }
}