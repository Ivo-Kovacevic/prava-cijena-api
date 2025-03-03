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

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        return await _context.Category
            .Where(c => c.Id == id)
            .Select(s => s.ToCategoryDto())
            .FirstOrDefaultAsync();
    }

    public async Task<CategoryDto> CreateAsync(Category category)
    {
        _context.Category.Add(category);
        await _context.SaveChangesAsync();
        return category.ToCategoryDto();
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id, Category category)
    {
        var existingCategory = await _context.Category.FirstOrDefaultAsync(c => c.Id == id);
        if (existingCategory == null)
        {
            return null;
        }
        existingCategory.Name = category.Name;
        existingCategory.Slug = category.Slug;
        existingCategory.ImageUrl = category.ImageUrl;
        existingCategory.ParentCategoryId = category.ParentCategoryId;
        
        await _context.SaveChangesAsync();

        return existingCategory.ToCategoryDto();
    }

    public async Task<CategoryDto?> DeleteAsync(Guid id)
    {
        var category = await _context.Category.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            return null;
        }

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();

        return category.ToCategoryDto();
    }
}