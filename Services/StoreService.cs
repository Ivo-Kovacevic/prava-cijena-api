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
        return await _storeRepo.GetAllAsync();
    }

    public async Task<StoreDto> GetStoreByIdAsync(Guid id)
    {
        var store = await _storeRepo.GetByIdAsync(id);
        if (store == null)
        {
            throw new NotFoundException($"Store with id '{id}' not found.");
        }

        return store;
    }

    public async Task<StoreDto> CreateStoreAsync(CreateStoreRequestDto storeRequestDto)
    {
        var store = storeRequestDto.StoreFromCreateRequestDto();
        return await _storeRepo.CreateAsync(store);
    }

    public async Task<StoreDto> UpdateStoreAsync(Guid id, UpdateStoreRequestDto storeRequestDto)
    {
        var store = storeRequestDto.StoreFromUpdateRequestDto();
        return await _storeRepo.UpdateAsync(id, store);
    }

    public async Task DeleteStoreAsync(Guid id)
    {
        await _storeRepo.DeleteAsync(id);
    }
}