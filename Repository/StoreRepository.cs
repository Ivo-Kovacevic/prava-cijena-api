using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Dto.StoreLocation;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Repository;

public class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _context;

    public StoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Store?> GetBySlugAsync(string storeSlug)
    {
        return await _context.Stores
            .Where(s => s.Slug == storeSlug)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Store>> GetAllAsync()
    {
        return await _context.Stores.ToListAsync();
    }

    public async Task<List<StoreWithMetadataDto>> GetAllWithMetadata()
    {
        var stores = await _context.Stores
            .Select(store => new StoreWithMetadataDto
            {
                Id = store.Id,
                Name = store.Name,
                Slug = store.Slug,
                StoreUrl = store.StoreUrl,
                PriceListUrl = store.PriceListUrl,
                PriceUrlListXPath = store.PriceUrlListXPath,
                PriceUrlXPath = store.PriceUrlXPath,
                PriceUrlType = store.PriceUrlType,
                DataLocation = store.DataLocation,
                CsvNameColumn = store.CsvNameColumn,
                CsvBrandColumn = store.CsvBrandColumn,
                CsvPriceColumn = store.CsvPriceColumn,
                CsvBarcodeColumn = store.CsvBarcodeColumn,
                CsvDelimiter = store.CsvDelimiter,
                XmlNameElement = store.XmlNameElement,
                XmlBrandElement = store.XmlBrandElement,
                XmlPriceElement = store.XmlPriceElement,
                XmlBarcodeElement = store.XmlBarcodeElement,
                BaseCategoryUrl = store.BaseCategoryUrl,
                ProductListXPath = store.ProductListXPath,
                CatalogueListUrl = store.CatalogueListUrl,
                CatalogueListXPath = store.CatalogueListXPath,
                PageQuery = store.PageQuery,
                LimitQuery = store.LimitQuery,
                ImageUrl = store.ImageUrl,
                Categories = store.Categories
                    .AsQueryable()
                    .Select(GetStoreCategoryProjection(4, 0))
                    .ToList(),
                StoreLocations = store.StoreLocations
                    .Select(location => new StoreLocationDto
                    {
                        Id = location.Id,
                        Address = location.Address,
                        City = location.City,
                        StoreId = location.StoreId
                    }).ToList()
            })
            .OrderBy(s => s.Name)
            .ToListAsync();

        return stores;
    }

    public async Task<Store?> GetByIdAsync(Guid id)
    {
        return await _context.Stores
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Store> CreateAsync(Store storeLocation)
    {
        _context.Stores.Add(storeLocation);
        await _context.SaveChangesAsync();
        return storeLocation;
    }

    public async Task<Store> UpdateAsync(Store existingStore)
    {
        _context.Stores.Update(existingStore);
        await _context.SaveChangesAsync();
        return existingStore;
    }

    public async Task DeleteAsync(Store existingStore)
    {
        _context.Stores.Remove(existingStore);
        await _context.SaveChangesAsync();
    }

    private static Expression<Func<StoreCategory, StoreCategoryDto>> GetStoreCategoryProjection(
        int maxDepth,
        int currentDepth = 0
    )
    {
        currentDepth++;

        Expression<Func<StoreCategory, StoreCategoryDto>> result = storeCategory => new StoreCategoryDto
        {
            Id = storeCategory.Id,
            Name = storeCategory.Name,
            StoreId = storeCategory.StoreId,
            ParentCategoryId = storeCategory.ParentCategoryId,
            EquivalentCategoryId = storeCategory.EquivalentCategoryId,
            Subcategories = currentDepth == maxDepth
                ? new List<StoreCategoryDto>()
                : storeCategory.Subcategories.AsQueryable()
                    .Select(GetStoreCategoryProjection(maxDepth, currentDepth))
                    .OrderBy(sc => sc.Name)
                    .ToList()
        };

        return result;
    }
}