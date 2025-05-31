using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StoreLocation>> GetAllStoreLocations(string userId)
    {
        var userCartProductIds = await _context.Cart
            .Where(c => c.UserId == userId)
            .Select(c => c.ProductId)
            .ToListAsync();

        return await _context.SavedStores
            .Where(sp => sp.UserId == userId)
            .Select(ss => new StoreLocation
            {
                Id = ss.StoreLocation.Id,
                StoreId = ss.StoreLocation.StoreId,
                City = ss.StoreLocation.City,
                Address = ss.StoreLocation.Address,
                Store = new Store
                {
                    Id = ss.StoreLocation.Store.Id,
                    Name = ss.StoreLocation.Store.Name,
                    StoreUrl = ss.StoreLocation.Store.StoreUrl,
                    ImageUrl = ss.StoreLocation.Store.ImageUrl
                },
                LocationProducts = ss.StoreLocation.LocationProducts
                    .Where(lp => userCartProductIds.Contains(lp.ProductId))
                    .Select(lp => new ProductStore
                    {
                        Id = lp.Id,
                        ProductId = lp.ProductId,
                        StoreLocationId = lp.StoreLocationId,
                        LatestPrice = lp.LatestPrice,
                        ProductUrl = lp.ProductUrl,
                        Product = new Product
                        {
                            Id = lp.Product.Id,
                            Name = lp.Product.Name,
                            Barcode = lp.Product.Barcode,
                            CategoryId = lp.Product.CategoryId
                        }
                    }).ToList()
            })
            .ToListAsync();
    }

    public async Task<List<Product>> GetAllProducts(string userId)
    {
        return await _context.Cart
            .Where(c => c.UserId == userId)
            .Select(c => new Product
            {
                Id = c.Product.Id,
                Barcode = c.Product.Barcode,
                CategoryId = c.Product.CategoryId,
                Name = c.Product.Name
            })
            .ToListAsync();
    }

    public async Task<Cart?> Get(string userId, Guid productId)
    {
        return await _context.Cart
            .Where(c => c.UserId == userId && c.ProductId == productId)
            .FirstOrDefaultAsync();
    }

    public async Task<Product> Create(Cart cart)
    {
        _context.Cart.Add(cart);
        await _context.SaveChangesAsync();

        return await _context.Cart
            .Where(c => c.ProductId == cart.ProductId)
            .Select(c => new Product
            {
                Id = c.Product.Id,
                Barcode = c.Product.Barcode,
                CategoryId = c.Product.CategoryId,
                Name = c.Product.Name
            })
            .FirstAsync();
    }

    public async Task Delete(Cart cart)
    {
        _context.Cart.Remove(cart);
        await _context.SaveChangesAsync();
    }
}