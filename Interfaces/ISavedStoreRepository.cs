using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface ISavedStoreRepository
{
    public Task<List<StoreLocation>> GetAll(string userId);
    public Task<SavedStore?> Get(string userId, Guid storeLocationId);
    public Task<StoreLocation> Create(SavedStore savedStore);
    public Task Delete(SavedStore savedStore);
}