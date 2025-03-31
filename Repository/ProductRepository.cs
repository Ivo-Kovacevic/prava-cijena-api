using api.Database;
using api.Interfaces;
using api.Models;
using Dapper;
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
        var sql = @"SELECT *, similarity(""Name"", @searchTerm) AS Similarity
                    FROM ""Products""
                    WHERE similarity(""Name"", @searchTerm) > 0.01
                    ORDER BY similarity(""Name"", @searchTerm) DESC";

        var connection = _context.Database.GetDbConnection();

        return await connection.QueryAsync<Product>(sql, new { searchTerm });
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