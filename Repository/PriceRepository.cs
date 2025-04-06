using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class PriceRepository : IPriceRepository
{
    private readonly AppDbContext _context;

    public PriceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Price>> GetPricesByProductStoreIdAsync(Guid productStoreId)
    {
        return await _context.Prices
            .Where(p => p.ProductStoreId == productStoreId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Price?> GetPriceByIdAsync(Guid priceId)
    {
        return await _context.Prices
            .Where(p => p.Id == priceId)
            .FirstOrDefaultAsync();
    }

    public async Task<Price> CreateAsync(Price price)
    {
        _context.Prices.Add(price);
        await _context.SaveChangesAsync();
        return price;
    }

    public async Task<Price> UpdateAsync(Price existingPrice)
    {
        _context.Prices.Add(existingPrice);
        await _context.SaveChangesAsync();
        return existingPrice;
    }

    public async Task DeleteAsync(Price existingPrice)
    {
        _context.Prices.Remove(existingPrice);
        await _context.SaveChangesAsync();
    }
}