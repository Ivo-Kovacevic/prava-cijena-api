using api.Dto.Store;

namespace api.Interfaces;

public interface IStoreService
{
    Task<IEnumerable<StoreDto>> GetStoresAsync();
    Task<StoreDto> GetStoreByIdAsync(Guid id);
    Task<StoreDto> CreateStoreAsync(CreateStoreRequestDto storeRequestDto);
    Task<StoreDto> UpdateStoreAsync(Guid id, UpdateStoreRequestDto storeRequestDto);
    Task DeleteStoreAsync(Guid id);
}