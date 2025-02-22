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
    public async Task<IEnumerable<Category>> GetAll()
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
    public async Task<IActionResult> Create(CreateCategoryRequestDto categoryDto)
    {
        var category = categoryDto.CreateCategoryFromDto();
        await _categoryRepo.CreateAsync(category);
        
        return CreatedAtAction(nameof(Get), new { slug = category.Slug }, category);
    }

    [HttpPut("{slug}")]
    public async Task<IActionResult> Update(string slug, UpdateCategoryRequestDto categoryDto)
    {
        var category = await _categoryRepo.UpdateAsync(slug, categoryDto);
        if (category == null)
        {
            return NotFound($"Category with slug '{slug}' not found.");
        }

        return Ok(category);
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        var category = await _categoryRepo.DeleteAsync(slug);
        if (category == null)
        {
            return NotFound($"Category with slug '{slug}' not found.");
        }

        return NoContent();
    }
}