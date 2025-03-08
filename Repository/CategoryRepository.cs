using api.Database;
using api.Dto.Category;
using api.Exceptions;
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
            .Select(c => c.ToCategoryDto())
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
        var affectedRows = await _context.Category
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(c => c.Name, category.Name)
                .SetProperty(c => c.Slug, category.Slug)
                .SetProperty(c => c.ImageUrl, category.ImageUrl)
                .SetProperty(c => c.ParentCategoryId, category.ParentCategoryId)
            );

        ThrowErrorIfNoRowsWereAffected(affectedRows, id);

        return category.ToCategoryDto();
    }

    public async Task DeleteAsync(Guid id)
    {
        var affectedRows = await _context.Category
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

        ThrowErrorIfNoRowsWereAffected(affectedRows, id);
    }

    public async Task<bool> CategoryExists(Guid id)
    {
        return await _context.Category.AnyAsync(c => c.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    private void ThrowErrorIfNoRowsWereAffected(int numOfAffectedRows, Guid id)
    {
        if (numOfAffectedRows == 0)
        {
            throw new NotFoundException($"Category with ID '{id}' not found.");
        }
    }
}