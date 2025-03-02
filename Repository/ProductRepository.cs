using api.Database;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Product>> GetProductsByCategoryAsync(string categorySlug)
    {
        return await _context.Product
            .Include(p => p.Category)
            .Where(p => p.Category.Slug == categorySlug)
            .ToListAsync();
    }

    public async Task<Product?> GetBySlugAsync(string slug)
    {
        return await _context.Product.SingleOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Product.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(int productId, Product product)
    {
        var existingProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == productId);
        if (existingProduct == null)
        {
            return null;
        }
        
        existingProduct.Name = product.Name;
        existingProduct.Slug = product.Slug;
        existingProduct.ImageUrl = product.ImageUrl;
        
        await _context.SaveChangesAsync();

        return existingProduct;
    }

    public async Task<Product?> DeleteAsync(int productId)
    {
        var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            return null;
        }

        _context.Remove(productId);
        await _context.SaveChangesAsync();

        return product;
    }
}