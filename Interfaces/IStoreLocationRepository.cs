using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IStoreLocationRepository
{
    Task<List<StoreLocation>> GetByProductAndStoreSlugAsync(string productSlug, string storeSlug);
    Task<StoreLocation?> GetByCityAndAddressAsync(string city, string address);
    Task<StoreLocation> Create(StoreLocation storeLocation);
}