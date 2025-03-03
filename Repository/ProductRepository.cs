using api.Database;
using api.DTOs;
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
    
    public async Task<List<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Product
            .Where(p => p.CategoryId == categoryId)
            .Select(p => p.ToProductDto())
            .ToListAsync();
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Product
            .Where(p => p.Id == productId)
            .Select(p => p.ToProductDto())
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto> CreateAsync(Product product)
    {
        _context.Product.Add(product);
        await _context.SaveChangesAsync();
        return product.ToProductDto();
    }

    public async Task<ProductDto?> UpdateAsync(Guid productId, Product product)
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

        return existingProduct.ToProductDto();
    }

    public async Task<ProductDto?> DeleteAsync(Guid productId)
    {
        var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            return null;
        }

        _context.Remove(productId);
        await _context.SaveChangesAsync();

        return product.ToProductDto();
    }
}