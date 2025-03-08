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

    public async Task<IEnumerable<Store>> GetAllAsync()
    {
        return await _context.Store.ToListAsync();
    }

    public async Task<Store?> GetByIdAsync(Guid id)
    {
        return await _context.Store
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Store> CreateAsync(Store store)
    {
        _context.Store.Add(store);
        await _context.SaveChangesAsync();
        return store;
    }

    public async Task<Store> UpdateAsync(Guid id, Store store)
    {
        var existingStore = await GetByIdAsync(id);
        if (existingStore == null)
        {
            throw new KeyNotFoundException($"Store with ID {id} not found.");
        }

        existingStore.Name = store.Name;
        existingStore.Slug = store.Slug;
        existingStore.StoreUrl = store.StoreUrl;
        existingStore.ImageUrl = store.ImageUrl;

        await _context.SaveChangesAsync();

        return store;
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