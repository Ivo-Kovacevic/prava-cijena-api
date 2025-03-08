using api.Database;
using api.Dto.Store;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _context;

    public StoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StoreDto>> GetAllAsync()
    {
        return await _context.Store
            .Select(s => s.ToStoreDto())
            .ToListAsync();
    }

    public async Task<StoreDto?> GetByIdAsync(Guid id)
    {
        return await _context.Store
            .Where(s => s.Id == id)
            .Select(s => s.ToStoreDto())
            .FirstOrDefaultAsync();
    }

    public async Task<StoreDto> CreateAsync(Store store)
    {
        _context.Store.Add(store);
        await _context.SaveChangesAsync();
        return store.ToStoreDto();
    }

    public async Task<StoreDto> UpdateAsync(Guid id, Store store)
    {
        _context.Store.Update(store);
        await _context.SaveChangesAsync();
        return store.ToStoreDto();
    }

    public async Task DeleteAsync(Guid id)
    {
        var affectedRows = await _context.Store
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

        ThrowErrorIfNoRowsWereAffected(affectedRows, id);
    }
    
    private void ThrowErrorIfNoRowsWereAffected(int numOfAffectedRows, Guid id)
    {
        if (numOfAffectedRows == 0)
        {
            throw new NotFoundException($"Store with ID '{id}' not found.");
        }
    }
}