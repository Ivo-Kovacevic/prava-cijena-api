using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class ProductStoreRepository : IProductStoreRepository
{
    private readonly AppDbContext _context;

    public ProductStoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductStore>> GetStoresByProductIdAsync(Guid productId)
    {
        return await _context.ProductStores
            .Where(ps => ps.ProductId == productId)
            .Include(ps => ps.StoreLocation)
            .ToListAsync();
    }

    public async Task<ProductStore?> GetProductStoreByIdsAsync(Guid productId, Guid storeLocationId)
    {
        return await _context.ProductStores
            .Where(ps => ps.ProductId == productId && ps.StoreLocationId == storeLocationId)
            .Include(ps => ps.StoreLocation)
            .FirstOrDefaultAsync();
    }

    public async Task<List<ProductStore>> GetProductStoresByProductBarcodesBatchAsync(List<string> barcodes,
        Guid storeLocationId)
    {
        return await _context.ProductStores
            .Where(ps => barcodes.Contains(ps.Product.Barcode) && ps.StoreLocationId == storeLocationId)
            .ToListAsync();
    }

    public async Task<ProductStore> CreateAsync(ProductStore productStore)
    {
        _context.ProductStores.Add(productStore);
        await _context.SaveChangesAsync();
        return productStore;
    }

    public async Task CreateProductStoresBatchAsync(List<ProductStore> productStores)
    {
        try
        {
            var now = DateTime.UtcNow;
            var valueLines = new List<string>();
            var parameters = new List<object>();

            for (var i = 0; i < productStores.Count; i++)
            {
                valueLines.Add(
                    $"(@p{i}_id, @p{i}_productId, @p{i}_storeLocationId, @p{i}_price, @p{i}_now, @p{i}_now)");

                parameters.Add(new NpgsqlParameter($"p{i}_id", Guid.NewGuid()));
                parameters.Add(new NpgsqlParameter($"p{i}_productId", productStores[i].ProductId));
                parameters.Add(new NpgsqlParameter($"p{i}_storeLocationId", productStores[i].StoreLocationId));
                parameters.Add(new NpgsqlParameter($"p{i}_price", productStores[i].LatestPrice));
                parameters.Add(new NpgsqlParameter($"p{i}_now", now));
            }

            var sql = $@"
            INSERT INTO ""ProductStores"" (
                ""Id"", ""ProductId"", ""StoreLocationId"",
                ""LatestPrice"", ""CreatedAt"", ""UpdatedAt""
            ) VALUES
            {string.Join(",\n", valueLines)}";

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task<ProductStore> UpdateAsync(ProductStore productStore)
    {
        _context.ProductStores.Update(productStore);
        await _context.SaveChangesAsync();
        return productStore;
    }

    public async Task UpdateProductStoresBatchAsync(List<ProductStore> productStores)
    {
        try
        {
            var updateLines = new List<string>();
            var parameters = new List<object>();

            for (var i = 0; i < productStores.Count; i++)
            {
                var paramPrice = new NpgsqlParameter($"p{i}_price", productStores[i].LatestPrice);
                var paramId = new NpgsqlParameter($"p{i}_id", productStores[i].Id);

                parameters.Add(paramPrice);
                parameters.Add(paramId);

                updateLines.Add($@"
                UPDATE ""ProductStores""
                SET ""LatestPrice"" = @p{i}_price,
                    ""UpdatedAt"" = NOW()
                WHERE ""Id"" = @p{i}_id;
            ");
            }

            var sql = string.Join("\n", updateLines);

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task UpdatePriceAsync(Guid productStoreId, decimal price)
    {
        await _context.ProductStores
            .Where(ps => ps.Id == productStoreId)
            .ExecuteUpdateAsync(set => set.SetProperty(ps => ps.LatestPrice, price));
    }

    public async Task DeleteAsync(ProductStore productStore)
    {
        _context.ProductStores.Remove(productStore);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ProductStoreExistsAsync(Guid productStoreId)
    {
        return await _context.ProductStores.AnyAsync(ps => ps.Id == productStoreId);
    }

    public async Task BulkCreateAsync(List<ProductStore> productStores)
    {
        await _context.BulkInsertAsync(productStores,
            new BulkConfig { PropertiesToExclude = new List<string> { nameof(ProductStore.Id) } });
    }

    public async Task BulkUpdateAsync(List<ProductStore> productStores)
    {
        await _context.BulkUpdateAsync(productStores);
    }
}