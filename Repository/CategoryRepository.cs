using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetBySlugAsync(string categorySlug)
    {
        return await _context.Categories
            .Where(c => c.Slug == categorySlug)
            .FirstOrDefaultAsync();
    }

    public async Task<Category?> GetBySlugWithFiltersAsync(string categorySlug)
    {
        return await _context.Categories
            .Where(c => c.Slug == categorySlug)
            .Include(c => c.Subcategories)
            .Include(c => c.Labels)
            .ThenInclude(l => l.Values)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.Subcategories)
            .Include(c => c.Labels)
            .ToListAsync();
    }

    public async Task<List<Category>> GetAllCategoriesWithSubcategoriesAsync()
    {
        return await _context.Categories
            .Where(c => !c.Subcategories.Any())
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid categoryId)
    {
        return await _context.Categories
            .Where(c => c.Id == categoryId)
            .FirstOrDefaultAsync();
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _context.Categories.Add(category);
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
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    private async Task LoadSubcategoriesRecursive(Category category)
    {
        await _context.Entry(category)
            .Collection(c => c.Subcategories)
            .LoadAsync();

        foreach (var subcategory in category.Subcategories) await LoadSubcategoriesRecursive(subcategory);
    }

    public async Task<bool> CategorySlugExists(string categorySlug)
    {
        return await _context.Categories.AnyAsync(c => c.Slug == categorySlug);
    }
}