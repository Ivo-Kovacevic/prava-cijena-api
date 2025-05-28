using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class SavedProductRepository : ISavedProductRepository
{
    private readonly AppDbContext _context;

    public SavedProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductWithMetadata>> GetAll(string userId)
    {
        var productsWithMetadata = await _context.SavedProducts
            .Where(sp => sp.UserId == userId)
            .Select(sp => new ProductWithMetadata
            {
                Id = sp.Product.Id,
                Name = sp.Product.Name,
                ImageUrl = sp.Product.ImageUrl,
                LowestPrice = sp.Product.LowestPrice,
                CreatedAt = sp.Product.CreatedAt,
                UpdatedAt = sp.Product.UpdatedAt,
                CategoryId = sp.Product.CategoryId,

                NumberOfStores = _context.ProductStores
                    .Where(ps => ps.ProductId == sp.Product.Id)
                    .Select(ps => ps.StoreLocation.StoreId)
                    .Distinct()
                    .Count(),
                SavedProduct = true
            })
            .ToListAsync();

        return productsWithMetadata;
    }

    public async Task<SavedProduct?> Get(string userId, Guid productId)
    {
        return await _context.SavedProducts
            .Where(sp => sp.UserId == userId && sp.ProductId == productId)
            .FirstOrDefaultAsync();
    }

    public async Task<SavedProduct> Create(SavedProduct savedProduct)
    {
        _context.SavedProducts.Add(savedProduct);
        await _context.SaveChangesAsync();
        return savedProduct;
    }

    public async Task Delete(SavedProduct savedProduct)
    {
        _context.SavedProducts.Remove(savedProduct);
        await _context.SaveChangesAsync();
    }
}