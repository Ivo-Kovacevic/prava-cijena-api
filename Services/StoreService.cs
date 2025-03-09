using api.Dto.Store;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;

namespace api.Services;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepo;

    public StoreService(IStoreRepository storeRepo)
    {
        _storeRepo = storeRepo;
    }

    public async Task<IEnumerable<StoreDto>> GetStoresAsync()
    {
        var stores = await _storeRepo.GetAllAsync();

        return stores.Select(s => s.ToStoreDto());
    }

    public async Task<StoreDto> GetStoreByIdAsync(Guid storeId)
    {
        var store = await _storeRepo.GetByIdAsync(storeId);
        if (store == null)
        {
            throw new NotFoundException($"Store with id '{storeId}' not found.");
        }

        return store.ToStoreDto();
    }

    public async Task<StoreDto> CreateStoreAsync(CreateStoreRequestDto storeRequestDto)
    {
        var store = storeRequestDto.StoreFromCreateRequestDto();
        store = await _storeRepo.CreateAsync(store);

        return store.ToStoreDto();
    }

    public async Task<StoreDto> UpdateStoreAsync(Guid storeId, UpdateStoreRequestDto storeRequestDto)
    {
        var existingStore = await _storeRepo.GetByIdAsync(storeId);
        if (existingStore == null)
        {
            throw new NotFoundException($"Store with id '{storeId}' not found.");
        }
        
        existingStore.StoreFromUpdateRequestDto(storeRequestDto);
        existingStore = await _storeRepo.UpdateAsync(existingStore);

        return existingStore.ToStoreDto();
    }

    public async Task DeleteStoreAsync(Guid storeId)
    {
        var existingStore = await _storeRepo.GetByIdAsync(storeId);
        if (existingStore == null)
        {
            throw new NotFoundException($"Store with id '{storeId}' not found.");
        }
        
        await _storeRepo.DeleteAsync(existingStore);
    }
}