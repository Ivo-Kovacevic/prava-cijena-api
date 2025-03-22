using api.Models;

namespace api.Interfaces;

public interface IProductStoreRepository
{
    Task<IEnumerable<ProductStore>> GetStoresByProductIdAsync(Guid productId);

    // Task<IEnumerable<ProductStore>> GetProductsByStoreIdAsync(Guid storeId);
    Task<ProductStore?> GetProductStoreByIdsAsync(Guid productId, Guid storeId);
    Task<ProductStore> CreateAsync(ProductStore productStore);
    Task<ProductStore> UpdateAsync(ProductStore existingProductStore);
    Task DeleteAsync(ProductStore existingProductStore);
    Task<bool> ProductStoreExistsAsync(Guid productStoreId);
}