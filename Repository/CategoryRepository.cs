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

    public async Task<Category?> GetByIdAsync(Guid categoryId)
    {
        return await _context.Category
            .Where(c => c.Id == categoryId)
            .FirstOrDefaultAsync();
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Category.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateAsync(Category existingCategory)
    {
        _context.Update(existingCategory);
        await _context.SaveChangesAsync();
        return existingCategory;
    }

    public async Task DeleteAsync(Category existingCategory)
    {
        _context.Remove(existingCategory);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CategoryExists(Guid id)
    {
        return await _context.Category.AnyAsync(c => c.Id == id);
    }
}