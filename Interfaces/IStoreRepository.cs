using api.Dto.Store;
using api.Models;

namespace api.Interfaces;

public interface IStoreRepository
{
    Task<IEnumerable<StoreDto>> GetAllAsync();
    Task<StoreDto?> GetByIdAsync(Guid id);
    Task<StoreDto> CreateAsync(Store category);
    Task<StoreDto> UpdateAsync(Guid id, Store category);
    Task DeleteAsync(Guid id);
}