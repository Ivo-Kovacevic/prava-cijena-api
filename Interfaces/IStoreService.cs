using PravaCijena.Api.Dto.Store;

namespace PravaCijena.Api.Interfaces;

public interface IStoreService
{
    Task<StoreDto> GetStoreBySlugAsync(string storeSlug);
    Task<IEnumerable<StoreDto>> GetStoresAsync();
    Task<StoreDto> GetStoreByIdAsync(Guid id);
    Task<StoreDto> CreateStoreAsync(CreateStoreRequestDto storeRequestDto);
    Task<StoreDto> UpdateStoreAsync(Guid id, UpdateStoreRequestDto storeRequestDto);
    Task DeleteStoreAsync(Guid id);
}