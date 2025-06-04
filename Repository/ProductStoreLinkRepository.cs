using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class ProductStoreLinkRepository : IProductStoreLinkRepository
{
    private readonly AppDbContext _context;

    public ProductStoreLinkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductStoreLink?> Get(Guid storeId, Guid productId)
    {
        return await _context.ProductStoreLinks
            .Where(psl => psl.StoreId == storeId && psl.ProductId == productId)
            .FirstOrDefaultAsync();
    }

    public async Task<ProductStoreLink> Create(ProductStoreLink productStoreLink)
    {
        _context.ProductStoreLinks.Add(productStoreLink);
        await _context.SaveChangesAsync();
        return productStoreLink;
    }
}