using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductBySlugAsync(string productSlug)
    {
        return await _context.Products
            .Where(p => p.Slug == productSlug)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProductWithSimilarityDto>> Search(string searchTerm)
    {
        var product = await _context.Database
            .SqlQuery<ProductWithSimilarityDto>(
                $@"SELECT *,
                          similarity(""Name"", {searchTerm}) AS ""Similarity""
                   FROM ""Products""
                   WHERE similarity(""Name"", {searchTerm}) > 0.05
                   ORDER BY similarity(""Name"", {searchTerm})
                   DESC"
            )
            .ToListAsync();

        return product;
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

    public async Task UpdateLowestPriceAsync(Guid productId, decimal price)
    {
        await _context.Products
            .Where(p => p.Id == productId)
            .ExecuteUpdateAsync(set => set.SetProperty(p => p.LowestPrice, price));
    }

    public async Task DeleteAsync(Product existingProduct)
    {
        _context.Products.Remove(existingProduct);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, QueryObject query)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Skip((query.Page - 1) * query.Limit)
            .Take(query.Limit)
            .ToListAsync();
    }
}