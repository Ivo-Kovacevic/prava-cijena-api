using api.Database;
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

    public async Task<List<Category>> GetAllRootCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid categoryId)
    {
        return await _context.Categories
            .Where(c => c.Id == categoryId)
            .FirstOrDefaultAsync();
    }

    public async Task<Category?> GetBySlugWithSubcategoriesAsync(string categorySlug)
    {
        var category = await _context.Categories
            .Where(c => c.Slug == categorySlug)
            .FirstOrDefaultAsync();
        if (category != null)
        {
            await LoadSubcategoriesRecursive(category);
        }

        return category;
    }
    
    private async Task LoadSubcategoriesRecursive(Category category)
    {
        await _context.Entry(category)
            .Collection(c => c.Subcategories)
            .LoadAsync();

        foreach (var subcategory in category.Subcategories)
        {
            await LoadSubcategoriesRecursive(subcategory);
        }
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
}