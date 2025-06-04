using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class StoreLocationRepository : IStoreLocationRepository
{
    private readonly AppDbContext _context;

    public StoreLocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StoreLocation>> GetByProductAndStoreSlugAsync(string productSlug, string storeSlug)
    {
        return await _context.StoreLocations
            .Where(sl =>
                sl.Store.Slug == storeSlug &&
                sl.LocationProducts.Any(ps => ps.Product.Slug == productSlug))
            .Include(sl => sl.LocationProducts
                .Where(ps => ps.Product.Slug == productSlug))
            .ToListAsync();
    }

    public async Task<List<StoreLocation>> GetByStoreIdAsync(Guid storeId)
    {
        return await _context.StoreLocations
            .Where(sl => sl.Store.Id == storeId)
            .OrderBy(sl => sl.City)
            .ToListAsync();
    }

    public async Task<StoreLocation?> GetByCityAndAddressAsync(string city, string address)
    {
        return await _context.StoreLocations
            .Where(sl => sl.City == city && sl.Address == address)
            .FirstOrDefaultAsync();
    }

    public async Task<StoreLocation> Create(StoreLocation storeLocation)
    {
        _context.ChangeTracker.Clear();
        _context.StoreLocations.Add(storeLocation);
        await _context.SaveChangesAsync();
        return storeLocation;
    }
}