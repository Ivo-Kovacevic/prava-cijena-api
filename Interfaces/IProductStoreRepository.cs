using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IProductStoreRepository
{
    Task<IEnumerable<ProductStore>> GetStoresByProductIdAsync(Guid productId);

    // Task<IEnumerable<ProductStore>> GetProductsByStoreIdAsync(Guid storeId);
    Task<ProductStore?> GetProductStoreByIdsAsync(Guid productId, Guid storeLocationId);
    Task<ProductStore> CreateAsync(ProductStore productStore);
    Task<ProductStore> UpdateAsync(ProductStore existingProductStore);
    Task UpdatePriceAsync(Guid productStoreId, decimal price);
    Task DeleteAsync(ProductStore existingProductStore);
    Task<bool> ProductStoreExistsAsync(Guid productStoreId);
}