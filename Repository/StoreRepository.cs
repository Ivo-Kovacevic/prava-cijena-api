using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _context;

    public StoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Store?> GetBySlugAsync(string storeSlug)
    {
        return await _context.Stores
            .Where(s => s.Slug == storeSlug)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Store>> GetAllAsync()
    {
        return await _context.Stores.ToListAsync();
    }

    public async Task<Store?> GetByIdAsync(Guid id)
    {
        return await _context.Stores
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Store> CreateAsync(Store store)
    {
        _context.Stores.Add(store);
        await _context.SaveChangesAsync();
        return store;
    }

    public async Task<Store> UpdateAsync(Store existingStore)
    {
        _context.Stores.Update(existingStore);
        await _context.SaveChangesAsync();
        return existingStore;
    }

    public async Task DeleteAsync(Store existingStore)
    {
        _context.Stores.Remove(existingStore);
        await _context.SaveChangesAsync();
    }
}