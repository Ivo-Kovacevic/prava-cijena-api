using api.Database;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _context.Category.ToListAsync();
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<Category>> Get(string slug)
    {
        var existingCategory = await _context.Category.SingleOrDefaultAsync(c => c.Slug == slug);
        if (existingCategory == null)
        {
            return NotFound($"Category with slug '{slug}' not found.");
        }

        return Ok(existingCategory);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create(Category category)
    {
        _context.Category.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { slug = category.Slug }, category);
    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<Category>> Update(string slug, Category category)
    {
        var existingCategory = await _context.Category.SingleOrDefaultAsync(c => c.Slug == slug);
        if (existingCategory == null)
        {
            return NotFound($"Category with slug '{slug}' not found.");
        }

        existingCategory.Name = category.Name;
        existingCategory.Slug = category.Slug;
        existingCategory.ImageUrl = category.ImageUrl;
        await _context.SaveChangesAsync();

        return Ok(existingCategory);
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        var existingCategory = await _context.Category.SingleOrDefaultAsync(c => c.Slug == slug);
        if (existingCategory == null)
        {
            return NotFound($"Category with slug '{slug}' not found.");
        }

        _context.Category.Remove(existingCategory);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}