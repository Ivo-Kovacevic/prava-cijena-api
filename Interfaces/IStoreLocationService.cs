using PravaCijena.Api.Dto.StoreLocation;

namespace PravaCijena.Api.Interfaces;

public interface IStoreLocationService
{
    Task<List<StoreLocationDto>> GetStorelocationsBySlug(string productSlug, string storeSlug);
}