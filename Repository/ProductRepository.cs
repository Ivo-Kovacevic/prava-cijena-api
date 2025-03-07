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

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId)
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

    public async Task<ProductDto> UpdateAsync(Guid productId, Product product)
    {
        var affectedRows = await _context.Product
            .Where(p => p.Id == productId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, product.Name)
                .SetProperty(p => p.Slug, product.Slug)
                .SetProperty(p => p.ImageUrl, product.ImageUrl)
                .SetProperty(p => p.CategoryId, product.CategoryId)
            );

        ThrowErrorIfNoRowsWereAffected(affectedRows, productId);

        return product.ToProductDto();
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