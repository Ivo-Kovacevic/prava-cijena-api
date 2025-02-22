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
    
    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Product.ToListAsync();
    }

    public async Task<Product?> GetBySlugAsync(string slug)
    {
        return await _context.Product.SingleOrDefaultAsync(p => p.Slug == slug);
    }
}