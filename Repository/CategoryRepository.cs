using api.Database;
using api.DTOs.Category;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public Task<List<Category>> GetAllAsync()
    {
        return _context.Category.ToListAsync();
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _context.Category.FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Category.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateAsync(string slug, UpdateCategoryRequestDto categoryDto)
    {
        var existingCategory = await _context.Category.FirstOrDefaultAsync(c => c.Slug == slug);
        if (existingCategory == null)
        {
            return null;
        }

        var name = categoryDto.Name.Trim();
        
        existingCategory.Name = name;
        existingCategory.Slug = SlugHelper.GenerateSlug(name);
        existingCategory.ImageUrl = categoryDto.ImageUrl;
        
        await _context.SaveChangesAsync();

        return existingCategory;
    }

    public async Task<Category?> DeleteAsync(string slug)
    {
        var category = await _context.Category.FirstOrDefaultAsync(c => c.Slug == slug);
        if (category == null)
        {
            return null;
        }

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();

        return category;
    }
}