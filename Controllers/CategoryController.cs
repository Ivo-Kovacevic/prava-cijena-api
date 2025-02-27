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
        try
        {
            var category = await _categoryRepo.GetBySlugAsync(slug);
            if (category is null)
            {
                return NotFound($"Category with slug '{slug}' not found.");
            }

            return Ok(category);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Error getting category");
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequestDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var category = categoryDto.CreateCategoryFromDto();
        await _categoryRepo.CreateAsync(category);
            
        return CreatedAtAction(nameof(Get), new { slug = category.Slug }, category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryRequestDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
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