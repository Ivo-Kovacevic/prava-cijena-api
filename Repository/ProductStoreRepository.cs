using api.Database;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class ProductStoreRepository : IProductStoreRepository
{
    private readonly AppDbContext _context;

    public ProductStoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductStore>> GetStoresByProductIdAsync(Guid productId)
    {
        return await _context.ProductStores
            .Where(ps => ps.ProductId == productId)
            .Include(ps => ps.Store)
            .ToListAsync();
    }

    public async Task<ProductStore?> GetProductStoreByIdsAsync(Guid productId, Guid storeId)
    {
        return await _context.ProductStores
            .Where(ps => ps.ProductId == productId && ps.StoreId == storeId)
            .Include(ps => ps.Store)
            .FirstOrDefaultAsync();
    }

    public async Task<ProductStore> CreateAsync(ProductStore productStore)
    {
        _context.ProductStores.Add(productStore);
        await _context.SaveChangesAsync();
        return productStore;
    }

    public async Task<ProductStore> UpdateAsync(ProductStore productStore)
    {
        _context.ProductStores.Update(productStore);
        await _context.SaveChangesAsync();
        return productStore;
    }

    public async Task DeleteAsync(ProductStore productStore)
    {
        _context.ProductStores.Remove(productStore);
        await _context.SaveChangesAsync();
    }
}