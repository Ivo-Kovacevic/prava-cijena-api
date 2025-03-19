using api.Database;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class OptionRepository : IOptionRepository
{
    private readonly AppDbContext _context;

    public OptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Option>> GetOptionsByAttributeIdAsync(Guid attributeId)
    {
        return await _context.Options
            .Where(o => o.AttributeId == attributeId)
            .ToListAsync();
    }

    public async Task<Option?> GetOptionByIdAsync(Guid attributeId)
    {
        return await _context.Options
            .Where(a => a.Id == attributeId)
            .FirstOrDefaultAsync();
    }

    public async Task<Option> CreateAsync(Option attribute)
    {
        _context.Options.Add(attribute);
        await _context.SaveChangesAsync();
        return attribute;
    }

    public async Task<Option> UpdateAsync(Option existingOption)
    {
        _context.Options.Update(existingOption);
        await _context.SaveChangesAsync();
        return existingOption;
    }

    public async Task DeleteAsync(Option existingOption)
    {
        _context.Options.Remove(existingOption);
        await _context.SaveChangesAsync();
    }
}