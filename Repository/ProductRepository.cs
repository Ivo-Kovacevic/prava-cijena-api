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

    public async Task<IEnumerable<ProductWithStoresNumber>> GetPageProductsByCategoryIdAsync(Guid categoryId,
        QueryObject query)
    {
        var subcategoryIds = await _context.Categories
            .Where(c => c.ParentCategoryId == categoryId)
            .Select(c => c.Id)
            .ToListAsync();

        var categoryIdsToSearch = subcategoryIds.Any() ? subcategoryIds : [categoryId];

        var productsWithStoreCounts = await _context.Products
            .Where(p => categoryIdsToSearch.Contains(p.CategoryId))
            .Where(p =>
                _context.ProductStores
                    .Where(ps => ps.ProductId == p.Id)
                    .Select(ps => ps.StoreLocation.StoreId)
                    .Distinct()
                    .Any()
            )
            .Select(p => new ProductWithStoresNumber
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                LowestPrice = p.LowestPrice,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                CategoryId = p.CategoryId,

                NumberOfStores = _context.ProductStores
                    .Where(ps => ps.ProductId == p.Id)
                    .Select(ps => ps.StoreLocation.StoreId)
                    .Distinct()
                    .Count()
            })
            .OrderByDescending(p => p.NumberOfStores)
            .Skip((query.Page - 1) * query.Limit)
            .Take(query.Limit)
            .ToListAsync();

        return productsWithStoreCounts;
    }

    public async Task<Product?> GetProductBySlugAsync(string productSlug)
    {
        return await _context.Products
            .Where(p => p.Slug == productSlug)
            .FirstOrDefaultAsync();
    }

    public async Task<List<StoreWithPriceDto>> GetProductStoresBySlugsAsync(string productSlug)
    {
        var productId = await _context.Products
            .Where(p => p.Slug == productSlug)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();

        if (productId == Guid.Empty)
        {
            return new List<StoreWithPriceDto>();
        }

        var grouped = await _context.ProductStores
            .Where(ps => ps.ProductId == productId)
            .Include(ps => ps.StoreLocation)
            .ThenInclude(sl => sl.Store)
            .ToListAsync();

        var result = grouped
            .GroupBy(ps => ps.StoreLocation.Store.Id)
            .Select(g =>
            {
                var cheapest = g.OrderBy(x => x.LatestPrice).First();
                var store = cheapest.StoreLocation.Store;

                return new StoreWithPriceDto
                {
                    Id = store.Id,
                    Name = store.Name,
                    StoreUrl = store.StoreUrl,
                    ImageUrl = store.ImageUrl,
                    Price = cheapest.LatestPrice,
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

    public async Task<IEnumerable<ProductWithSimilarityDto>> Search(string searchTerm, int page,
        int limit)
    {
        var offset = (page - 1) * limit;


        var products = await _context.Database
            .SqlQuery<ProductWithSimilarityDto>(
                $@"SELECT *, similarity(""Name"", {searchTerm}) AS ""Similarity""
                   FROM ""Products""
                   WHERE similarity(""Name"", {searchTerm}) > 0.3
                   ORDER BY similarity(""Name"", {searchTerm}) DESC
                   LIMIT {limit}
                   OFFSET {offset}"
            )
            .ToListAsync();

        // var totalProducts = await _context.Database
        //     .SqlQuery<int>(
        //         $@"SELECT COUNT(*) AS ""Value""
        //            FROM ""Products""
        //            WHERE similarity(""Name"", {searchTerm}) > 0.3"
        //     )
        //     .FirstOrDefaultAsync();
        //
        // var totalPages = (int)Math.Ceiling(totalProducts / (double)limit);

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
}