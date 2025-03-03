using api.DTOs.Category;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/categories")]
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound($"Category with id '{id}' not found.");
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
            var parentCategoryExists = await _categoryRepo.CategoryExists(categoryRequestDto.ParentCategoryId.Value);
            if (!parentCategoryExists)
            {
                return NotFound($"Parent category with id '{categoryRequestDto.ParentCategoryId}' not found.");
            }
        }

        var category = categoryRequestDto.CategoryFromCreateRequestDto();
        var createdCategory = await _categoryRepo.CreateAsync(category);
            
        return CreatedAtAction(nameof(Get), new { id = category.Id }, createdCategory);
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
            var parentCategoryExists = await _categoryRepo.CategoryExists(categoryRequestDto.ParentCategoryId.Value);
            if (!parentCategoryExists)
            {
                return NotFound($"Parent category with id '{categoryRequestDto.ParentCategoryId}' not found.");
            }
        }
        
        var category = categoryRequestDto.CategoryFromUpdateRequestDto();

        var updatedCategory = await _categoryRepo.UpdateAsync(id, category);
        if (updatedCategory == null)
        {
            return NotFound($"Category with id '{id}' not found.");
        }

        return Ok(updatedCategory);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var category = await _categoryRepo.DeleteAsync(id);
        if (category == null)
        {
            return NotFound($"Category with id '{id}' not found.");
        }

        return NoContent();
    }
}