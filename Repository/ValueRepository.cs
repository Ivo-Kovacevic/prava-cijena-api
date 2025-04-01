using api.Database;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class ValueRepository : IValueRepository
{
    private readonly AppDbContext _context;

    public ValueRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Value>> GetValuesByLabelIdAsync(Guid attributeId)
    {
        return await _context.Values
            .Where(o => o.LabelId == attributeId)
            .ToListAsync();
    }

    public async Task<Value?> GetValueByIdAsync(Guid attributeId)
    {
        return await _context.Values
            .Where(a => a.Id == attributeId)
            .FirstOrDefaultAsync();
    }

    public async Task<Value> CreateAsync(Value value)
    {
        _context.Values.Add(value);
        await _context.SaveChangesAsync();
        return value;
    }

    public async Task<Value> UpdateAsync(Value existingValue)
    {
        _context.Values.Update(existingValue);
        await _context.SaveChangesAsync();
        return existingValue;
    }

    public async Task DeleteAsync(Value existingValue)
    {
        _context.Values.Remove(existingValue);
        await _context.SaveChangesAsync();
    }
}