using PravaCijena.Api.Exceptions;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Services;

public class SavedStoreService : ISavedStoreService
{
    private readonly ISavedStoreRepository _savedStoreRepository;

    public SavedStoreService(ISavedStoreRepository savedStoreRepository)
    {
        _savedStoreRepository = savedStoreRepository;
    }

    public async Task<List<StoreLocation>> GetAll(string userId)
    {
        var savedStores = await _savedStoreRepository.GetAll(userId);

        return savedStores;
    }

    public async Task<StoreLocation> Store(string userId, Guid storeLocationId)
    {
        var savedStore = await _savedStoreRepository.Create(new SavedStore
        {
            UserId = userId,
            StoreLocationId = storeLocationId
        });

        return savedStore;
    }

    public async Task<SavedStore?> Destroy(string userId, Guid storeLocationId)
    {
        var existingSavedStore = await _savedStoreRepository.Get(userId, storeLocationId);
        if (existingSavedStore == null)
        {
            return null;
        }

        await _savedStoreRepository.Delete(existingSavedStore);
        return existingSavedStore;
    }
}