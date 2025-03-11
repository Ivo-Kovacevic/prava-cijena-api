using api.Database;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Attribute = api.Models.Attribute;

namespace api.Repository;

public class AttributeRepository : IAttributeRepository
{
    private readonly AppDbContext _context;

    public AttributeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Attribute>> GetAttributesByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Attributes
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Attribute?> GetAttributeByIdAsync(Guid attributeId)
    {
        return await _context.Attributes
            .Where(a => a.Id == attributeId)
            .FirstOrDefaultAsync();
    }

    public async Task<Attribute> CreateAsync(Attribute attribute)
    {
        _context.Attributes.Add(attribute);
        await _context.SaveChangesAsync();
        return attribute;
    }

    public async Task<Attribute> UpdateAsync(Attribute existingAttribute)
    {
        _context.Attributes.Update(existingAttribute);
        await _context.SaveChangesAsync();
        return existingAttribute;
    }

    public async Task DeleteAsync(Attribute existingAttribute)
    {
        _context.Attributes.Remove(existingAttribute);
        await _context.SaveChangesAsync();
    }
}