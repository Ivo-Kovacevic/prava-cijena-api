using api.Database;
using api.DTOs.Category;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
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
    
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _context.Category
            .Select(c => c.ToCategoryDto())
            .ToListAsync();
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _context.Category.FirstOrDefaultAsync();
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Category.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateAsync(int id, Category categoryDto)
    {
        var existingCategory = await _context.Category.FirstOrDefaultAsync(c => c.Id == id);
        if (existingCategory == null)
        {
            return null;
        }
        existingCategory.Name = categoryDto.Name;
        existingCategory.Slug = categoryDto.Slug;
        existingCategory.ImageUrl = categoryDto.ImageUrl;
        
        await _context.SaveChangesAsync();

        return existingCategory;
    }

    public async Task<Category?> DeleteAsync(int id)
    {
        var category = await _context.Category.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            return null;
        }

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();

        return category;
    }
}