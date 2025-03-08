using api.Database;
using api.Dto.Product;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;
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
        return await _context.Product
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Product
            .Where(p => p.Id == productId)
            .FirstOrDefaultAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Product.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Guid productId, Product product)
    {
        var existingProduct = await GetProductByIdAsync(productId);
        if (existingProduct == null)
        {
            throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }

        existingProduct.Name = product.Name;
        existingProduct.Slug = product.Slug;
        existingProduct.ImageUrl = product.ImageUrl;
        existingProduct.CategoryId = product.CategoryId;

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task DeleteAsync(Guid productId)
    {
        var affectedRows = await _context.Product
            .Where(p => p.Id == productId)
            .ExecuteDeleteAsync();

        ThrowErrorIfNoRowsWereAffected(affectedRows, productId);
    }

    private void ThrowErrorIfNoRowsWereAffected(int numOfAffectedRows, Guid id)
    {
        if (numOfAffectedRows == 0)
        {
            throw new NotFoundException($"Product with ID '{id}' not found.");
        }
    }
}