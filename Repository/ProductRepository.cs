using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductBySlugAsync(string productSlug)
    {
        return await _context.Products
            .Where(p => p.Slug == productSlug)
            .FirstOrDefaultAsync();
    }

    public async Task<List<StoreWithPageInfoDto>> GetProductStoresBySlugsAsync(string productSlug)
    {
        var productId = await _context.Products
            .Where(p => p.Slug == productSlug)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();

        if (productId == Guid.Empty)
        {
            return new List<StoreWithPageInfoDto>();
        }

        var grouped = await _context.ProductStores
            .Where(ps => ps.ProductId == productId)
            .Include(ps => ps.StoreLocation)
            .ThenInclude(sl => sl.Store)
            .ToListAsync();
        
        var productStoreLinks = await _context.ProductStoreLinks
            .Where(link => link.ProductId == productId)
            .ToListAsync();

        var result = grouped
            .GroupBy(ps => ps.StoreLocation.Store.Id)
            .Select(g =>
            {
                var cheapest = g.OrderBy(x => x.LatestPrice).First();
                var store = cheapest.StoreLocation.Store;

                return new StoreWithPageInfoDto
                {
                    Id = store.Id,
                    Name = store.Name,
                    StoreUrl = store.StoreUrl,
                    ImageUrl = store.ImageUrl,
                    Price = cheapest.LatestPrice,
                    ProductUrl = productStoreLinks
                        .FirstOrDefault(link =>
                            link.StoreId == store.Id &&
                            link.ProductId == productId
                        )?.ProductLink,
                    CreatedAt = cheapest.CreatedAt,
                    UpdatedAt = cheapest.UpdatedAt
                };
            })
            .OrderBy(x => x.Price)
            .ToList();

        return result;
    }

    public async Task<List<string>> GetAllSlugsAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Select(p => p.Slug)
            .ToListAsync();
    }

    public async Task<List<string>> GetAllBarcodesAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Select(p => p.Barcode)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByBarcodeAsync(string productBarcode)
    {
        return await _context.Products
            .Where(p => p.Barcode == productBarcode)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Product>> GetProductsByBarcodesBatchAsync(IEnumerable<string> barcodes)
    {
        return await _context.Products
            .Where(p => barcodes.Contains(p.Barcode))
            .ToListAsync();
    }

    public async Task<List<Product>> Search(string searchTerm, int page,
        int limit)
    {
        var offset = (page - 1) * limit;

        var products = await _context.Database
            .SqlQuery<Product>(
                $@"SELECT *
                   FROM ""Products""
                   WHERE similarity(""Name"", {searchTerm}) > 0.3
                   ORDER BY similarity(""Name"", {searchTerm}) DESC
                   LIMIT {limit}
                   OFFSET {offset}"
            ).ToListAsync();

        return products;
    }

    public async Task<List<ProductWithMetadata>> SearchPageProducts(string searchTerm, int page, int limit)
    {
        var offset = (page - 1) * limit;

        var products = await _context.Database
            .SqlQuery<ProductWithMetadata>(
                $@"SELECT
                   p.*,
                   (
                       SELECT COUNT(DISTINCT sl.""StoreId"")
                       FROM ""ProductStores"" ps
                       JOIN ""StoreLocations"" sl ON ps.""StoreLocationId"" = sl.""Id""
                       WHERE ps.""ProductId"" = p.""Id""
                   ) AS ""NumberOfStores""
                   FROM ""Products"" p
                   WHERE similarity(p.""Name"", {searchTerm}) > 0.3
                   ORDER BY similarity(p.""Name"", {searchTerm}) DESC
                   LIMIT {limit}
                   OFFSET {offset}"
            ).ToListAsync();

        return products;
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Products
            .Where(p => p.Id == productId)
            .FirstOrDefaultAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.ChangeTracker.Clear();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product existingProduct)
    {
        _context.Products.Update(existingProduct);
        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task UpdateLowestPriceAsync(Guid productId, decimal price)
    {
        await _context.Products
            .Where(p => p.Id == productId)
            .ExecuteUpdateAsync(set => set.SetProperty(p => p.LowestPrice, price));
    }

    public async Task DeleteAsync(Product existingProduct)
    {
        _context.Products.Remove(existingProduct);
        await _context.SaveChangesAsync();
    }

    public async Task BulkCreateAsync(List<Product> products)
    {
        await _context.BulkInsertAsync(products,
            new BulkConfig { PropertiesToExclude = new List<string> { nameof(ProductStore.Id) } });
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, QueryObject query)
    {
        var subcategoryIds = await _context.Categories
            .Where(c => c.ParentCategoryId == categoryId)
            .Select(c => c.Id)
            .ToListAsync();

        var categoryIdsToSearch = subcategoryIds.Any() ? subcategoryIds : [categoryId];

        return await _context.Products
            .Where(p => categoryIdsToSearch.Contains(p.CategoryId))
            .Skip((query.Page - 1) * query.Limit)
            .Take(query.Limit)
            .OrderBy(p => p.LowestPrice)
            .ToListAsync();
    }

    public async Task BulkUpdateAsync(List<Product> products)
    {
        await _context.BulkUpdateAsync(products);
    }

    public async Task<Pagination> GetPageProductsByCategoryIdAsync(
        Guid categoryId,
        string? userId,
        QueryObject query
    )
    {
        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine("START");
        var subcategoryIds = await _context.Categories
            .Where(c => c.ParentCategoryId == categoryId)
            .Select(c => c.Id)
            .ToListAsync();

        var categoryIdsToSearch = subcategoryIds.Any() ? subcategoryIds : [categoryId];

        var baseProducts = await _context.Products
            .Where(p =>
                categoryIdsToSearch.Contains(p.CategoryId) &&
                p.ProductStores
                    .Select(ps => ps.StoreLocation.StoreId)
                    .Distinct()
                    .Any()
            )
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.ImageUrl,
                p.LowestPrice,
                p.CreatedAt,
                p.UpdatedAt,
                p.CategoryId,
                SavedProduct = userId != null && _context.SavedProducts
                    .Any(sp => sp.ProductId == p.Id && sp.UserId == userId)
            })
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)baseProducts.Count / query.Limit);

        var productStoreCounts = await _context.ProductStores
            .Where(ps => baseProducts.Select(p => p.Id).Contains(ps.ProductId))
            .Select(ps => new
            {
                ps.ProductId,
                ps.StoreLocation.StoreId
            })
            .Distinct()
            .GroupBy(x => x.ProductId)
            .ToDictionaryAsync(g => g.Key, g => g.Count());

        var productsWithMetadata = baseProducts
            .Select(p => new ProductWithMetadata
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                LowestPrice = p.LowestPrice,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                CategoryId = p.CategoryId,
                NumberOfStores = productStoreCounts.TryGetValue(p.Id, out var count) ? count : 0,
                // SavedProduct = p.SavedProduct
            })
            .OrderByDescending(p => p.NumberOfStores)
            .ThenBy(p => p.LowestPrice)
            .Skip((query.Page - 1) * query.Limit)
            .Take(query.Limit)
            .ToList();

        Console.WriteLine("END");
        Console.WriteLine("-----------------------------------");

        return new Pagination
        {
            CurrentPage = query.Page,
            TotalPages = totalPages,
            Products = productsWithMetadata
        };
    }
}