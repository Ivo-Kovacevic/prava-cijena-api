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

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Category.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Category
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Category.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateAsync(Guid id, Category category)
    {
        var existingCategory = await _context.Category
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        existingCategory.Name = category.Name;
        existingCategory.Slug = category.Slug;
        existingCategory.ImageUrl = category.ImageUrl;
        existingCategory.ParentCategoryId = category.ParentCategoryId;
        
        await _context.SaveChangesAsync();

        return existingCategory;
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