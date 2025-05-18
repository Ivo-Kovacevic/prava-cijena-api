using Microsoft.EntityFrameworkCore;
using Npgsql;
using PravaCijena.Api.Database;
using PravaCijena.Api.Dto.Product;
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
        var product = await _context.Products
            .Include(p => p.ProductStores)
            .ThenInclude(ps => ps.StoreLocation)
            .FirstOrDefaultAsync(p => p.Slug == productSlug);

        if (product != null)
        {
            product.ProductStores = product.ProductStores
                .OrderBy(ps => ps.LatestPrice)
                .ToList();
        }

        return product;
    }

    public async Task<List<Product>> GetProductsBySlugsBatchAsync(IEnumerable<string> productSlugs)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => productSlugs.Contains(p.Slug))
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
            .AsNoTracking()
            .Where(p => barcodes.Contains(p.Barcode))
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductWithSimilarityDto>> Search(string searchTerm)
    {
        var product = await _context.Database
            .SqlQuery<ProductWithSimilarityDto>(
                $@"SELECT *,
                          similarity(""Name"", {searchTerm}) AS ""Similarity""
                   FROM ""Products""
                   WHERE similarity(""Name"", {searchTerm}) > 0.00
                   ORDER BY similarity(""Name"", {searchTerm}) DESC
                   LIMIT 5"
            )
            .ToListAsync();

        return product;
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

    public async Task CreateProductsBatchAsync(List<Product> products)
    {
        try
        {
            var now = DateTime.UtcNow;
            var valueLines = new List<string>();
            var parameters = new List<object>();

            for (var i = 0; i < products.Count; i++)
            {
                var slug = SlugHelper.GenerateSlug(products[i].Name);

                valueLines.Add(
                    $"(@p{i}_id, NULL, @p{i}_barcode, @p{i}_category, @p{i}_price, @p{i}_now, @p{i}_now, @p{i}_name, @p{i}_slug)");

                parameters.Add(new NpgsqlParameter($"p{i}_id", Guid.NewGuid()));
                parameters.Add(new NpgsqlParameter($"p{i}_barcode", products[i].Barcode));
                parameters.Add(new NpgsqlParameter($"p{i}_category", products[i].CategoryId));
                parameters.Add(new NpgsqlParameter($"p{i}_price", products[i].LowestPrice));
                parameters.Add(new NpgsqlParameter($"p{i}_now", now));
                parameters.Add(new NpgsqlParameter($"p{i}_name", products[i].Name));
                parameters.Add(new NpgsqlParameter($"p{i}_slug", products[i].Slug));
            }

            var sql = $@"
            INSERT INTO ""Products"" (
                ""Id"", ""ImageUrl"", ""Barcode"", ""CategoryId"",
                ""LowestPrice"", ""CreatedAt"", ""UpdatedAt"",
                ""Name"", ""Slug""
            ) VALUES
            {string.Join(",\n", valueLines)}
        ";

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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

    public async Task UpdateLowestPricesBatchAsync(List<ProductPreview> productPreviews)
    {
        var now = DateTime.UtcNow;
        var valueLines = new List<string>();
        var parameters = new List<object>();

        for (var i = 0; i < productPreviews.Count; i++)
        {
            valueLines.Add($"(@p{i}_barcode, @p{i}_price, @p{i}_now)");
            parameters.Add(new NpgsqlParameter($"p{i}_barcode", productPreviews[i].Barcode));
            parameters.Add(new NpgsqlParameter($"p{i}_price", productPreviews[i].Price));
            parameters.Add(new NpgsqlParameter($"p{i}_now", now));
        }

        var sql = $@"
            UPDATE ""Products"" AS p
            SET ""LowestPrice"" = u.""NewPrice"",
                ""UpdatedAt"" = u.""Now""
            FROM (VALUES
                {string.Join(",\n", valueLines)}
            ) AS u(""Barcode"", ""NewPrice"", ""Now"")
            WHERE u.""Barcode"" = p.""Barcode""
        ";

        await _context.Database.ExecuteSqlRawAsync(sql, parameters);
    }

    public async Task DeleteAsync(Product existingProduct)
    {
        _context.Products.Remove(existingProduct);
        await _context.SaveChangesAsync();
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
}