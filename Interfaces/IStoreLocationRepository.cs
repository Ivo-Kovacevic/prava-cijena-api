using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IStoreLocationRepository
{
    Task<List<StoreLocation>> GetByProductAndStoreSlugAsync(string productSlug, string storeSlug);
    Task<List<StoreLocation>> GetByStoreIdAsync(Guid storeId);
    Task<StoreLocation?> GetByCityAndAddressAsync(string city, string address);
    Task<StoreLocation> Create(StoreLocation storeLocation);
}