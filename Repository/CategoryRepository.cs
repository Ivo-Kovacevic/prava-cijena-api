using api.Database;
using api.DTOs.Category;
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

    public async Task<CategoryDto> UpdateAsync(Guid id, Category category)
    {
        await _context.Category
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(c => c.Name, category.Name)
                .SetProperty(c => c.Slug, category.Slug)
                .SetProperty(c => c.ImageUrl, category.ImageUrl)
                .SetProperty(c => c.ParentCategoryId, category.ParentCategoryId));

        return category.ToCategoryDto();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Category
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<bool> CategoryExists(Guid id)
    {
        return await _context.Category.AnyAsync(c => c.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}