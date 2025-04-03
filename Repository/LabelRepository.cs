using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class LabelRepository : ILabelRepository
{
    private readonly AppDbContext _context;

    public LabelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Label?> GetLabelBySlugAsync(string labelSlug)
    {
        return await _context.Labels
            .Where(a => a.Slug == labelSlug)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Label>> GetLabelsByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Labels
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Label?> GetLabelByIdAsync(Guid attributeId)
    {
        return await _context.Labels
            .Where(a => a.Id == attributeId)
            .FirstOrDefaultAsync();
    }

    public async Task<Label> CreateAsync(Label label)
    {
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();
        return label;
    }

    public async Task<Label> UpdateAsync(Label existingLabel)
    {
        _context.Labels.Update(existingLabel);
        await _context.SaveChangesAsync();
        return existingLabel;
    }

    public async Task DeleteAsync(Label existingLabel)
    {
        _context.Labels.Remove(existingLabel);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> LabelExists(Guid attributeId)
    {
        return await _context.Labels.AnyAsync(a => a.Id == attributeId);
    }
}