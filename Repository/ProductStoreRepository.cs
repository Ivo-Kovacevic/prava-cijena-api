using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

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
            .Include(ps => ps.StoreLocation)
            .ToListAsync();
    }

    public async Task<ProductStore?> GetProductStoreByIdsAsync(Guid productId, Guid storeLocationId)
    {
        return await _context.ProductStores
            .Where(ps => ps.ProductId == productId && ps.StoreLocationId == storeLocationId)
            .Include(ps => ps.StoreLocation)
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

    public async Task UpdatePriceAsync(Guid productStoreId, decimal price)
    {
        await _context.ProductStores
            .Where(ps => ps.Id == productStoreId)
            .ExecuteUpdateAsync(set => set.SetProperty(ps => ps.LatestPrice, price));
    }

    public async Task DeleteAsync(ProductStore productStore)
    {
        _context.ProductStores.Remove(productStore);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ProductStoreExistsAsync(Guid productStoreId)
    {
        return await _context.ProductStores.AnyAsync(ps => ps.Id == productStoreId);
    }
}