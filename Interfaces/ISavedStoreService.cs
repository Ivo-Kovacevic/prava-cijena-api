using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface ISavedStoreService
{
    public Task<List<StoreLocation>> GetAll(string userId);
    public Task<StoreLocation> Store(string userId, Guid storeLocationId);
    public Task<SavedStore?> Destroy(string userId, Guid storeLocationId);
}