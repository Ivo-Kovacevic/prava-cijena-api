using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IStoreRepository
{
    Task<Store?> GetBySlugAsync(string storeSlug);
    Task<IEnumerable<Store>> GetAllAsync();
    Task<List<StoreWithMetadataDto>> GetAllWithMetadata();
    Task<Store?> GetByIdAsync(Guid id);
    Task<Store> CreateAsync(Store category);
    Task<Store> UpdateAsync(Store existingStore);
    Task DeleteAsync(Store existingStore);
}