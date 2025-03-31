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

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Product?> GetProductBySlugAsync(string productSlug)
    {
        return await _context.Products
            .Where(p => p.Slug == productSlug)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> Search(string searchTerm)
    {
        var productsWithSimilarity = await _context.Products
            .FromSqlRaw(@"
                SELECT *, similarity(""Name"", {0}) AS Similarity
                FROM ""Products""
                WHERE similarity(""Name"", {0}) > 0.01
                ORDER BY similarity(""Name"", {0}) DESC", searchTerm
            )
            .ToListAsync();

        return productsWithSimilarity;
    }


    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Products
            .Where(p => p.Id == productId)
            .FirstOrDefaultAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product existingProduct)
    {
        _context.Products.Update(existingProduct);
        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task DeleteAsync(Product existingProduct)
    {
        _context.Products.Remove(existingProduct);
        await _context.SaveChangesAsync();
    }
}