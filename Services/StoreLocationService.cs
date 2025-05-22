using PravaCijena.Api.Dto.StoreLocation;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Mappers;

namespace PravaCijena.Api.Services;

public class StoreLocationService : IStoreLocationService
{
    private readonly IStoreLocationRepository _storeLocationRepository;

    public StoreLocationService(IStoreLocationRepository storeLocationRepository)
    {
        _storeLocationRepository = storeLocationRepository;
    }

    public async Task<List<StoreLocationDto>> GetStorelocationsBySlug(string productSlug, string storeSlug)
    {
        var storeLocations = await _storeLocationRepository.GetByProductAndStoreSlugAsync(productSlug, storeSlug);

        return storeLocations.Select(sl => sl.ToStoreLocationDto()).ToList();
    }
}