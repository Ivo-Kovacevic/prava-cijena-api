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

    public async Task<StoreDto> GetStoreByIdAsync(Guid id)
    {
        var store = await _storeRepo.GetByIdAsync(id);
        if (store == null)
        {
            throw new NotFoundException($"Store with id '{id}' not found.");
        }

        return store.ToStoreDto();
    }

    public async Task<StoreDto> CreateStoreAsync(CreateStoreRequestDto storeRequestDto)
    {
        var store = storeRequestDto.StoreFromCreateRequestDto();
        store = await _storeRepo.CreateAsync(store);

        return store.ToStoreDto();
    }

    public async Task<StoreDto> UpdateStoreAsync(Guid id, UpdateStoreRequestDto storeRequestDto)
    {
        var store = storeRequestDto.StoreFromUpdateRequestDto();
        store = await _storeRepo.UpdateAsync(id, store);

        return store.ToStoreDto();
    }

    public async Task DeleteStoreAsync(Guid id)
    {
        await _storeRepo.DeleteAsync(id);
    }
}