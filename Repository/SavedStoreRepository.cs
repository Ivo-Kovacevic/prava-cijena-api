using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class SavedStoreRepository : ISavedStoreRepository
{
    private readonly AppDbContext _context;

    public SavedStoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StoreLocation>> GetAll(string userId)
    {
        return await _context.SavedStores
            .Where(sp => sp.UserId == userId)
            .Select(ss => new StoreLocation {
                Id = ss.StoreLocation.Id,
                StoreId = ss.StoreLocation.StoreId,
                City = ss.StoreLocation.City,
                Address = ss.StoreLocation.Address,
                Store = new Store
                {
                    Id = ss.StoreLocation.Store.Id,
                    Name = ss.StoreLocation.Store.Name,
                    StoreUrl = ss.StoreLocation.Store.StoreUrl,
                    ImageUrl = ss.StoreLocation.Store.ImageUrl
                }
            })
            .ToListAsync();
    }

    public async Task<SavedStore?> Get(string userId, Guid storeLocationId)
    {
        return await _context.SavedStores
            .Where(ss => ss.UserId == userId && ss.StoreLocationId == storeLocationId)
            .FirstOrDefaultAsync();
    }

    public async Task<StoreLocation> Create(SavedStore savedStore)
    {
        _context.SavedStores.Add(savedStore);
        await _context.SaveChangesAsync();

        return await _context.SavedStores
            .Where(ss => ss.StoreLocationId == savedStore.StoreLocationId)
            .Select(ss => new StoreLocation {
                Id = ss.StoreLocation.Id,
                StoreId = ss.StoreLocation.StoreId,
                City = ss.StoreLocation.City,
                Address = ss.StoreLocation.Address,
                Store = new Store
                {
                    Id = ss.StoreLocation.Store.Id,
                    Name = ss.StoreLocation.Store.Name,
                    StoreUrl = ss.StoreLocation.Store.StoreUrl,
                    ImageUrl = ss.StoreLocation.Store.ImageUrl
                }
            })
            .FirstAsync();
    }

    public async Task Delete(SavedStore savedStore)
    {
        _context.SavedStores.Remove(savedStore);
        await _context.SaveChangesAsync();
    }
}