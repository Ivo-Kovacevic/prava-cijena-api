using api.Dto.Store;
using api.Models;

namespace api.Interfaces;

public interface IStoreRepository
{
    Task<IEnumerable<Store>> GetAllAsync();
    Task<Store?> GetByIdAsync(Guid id);
    Task<Store> CreateAsync(Store category);
    Task<Store> UpdateAsync(Store existingStore);
    Task DeleteAsync(Store existingStore);
}