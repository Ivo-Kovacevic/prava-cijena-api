using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IProductStoreRepository
{
    Task<IEnumerable<ProductStore>> GetStoresByProductIdAsync(Guid productId);

    // Task<IEnumerable<ProductStore>> GetProductsByStoreIdAsync(Guid storeId);
    Task<ProductStore?> GetProductStoreByIdsAsync(Guid productId, Guid storeLocationId);
    Task<List<ProductStore>> GetProductStoresByIdsBatchAsync(List<Guid> productIds, Guid storeLocationId);
    Task<ProductStore> CreateAsync(ProductStore productStore);
    Task CreateProductStoresBatchAsync(List<ProductStore> productStores);
    Task<ProductStore> UpdateAsync(ProductStore existingProductStore);
    Task UpdateProductStoresBatchAsync(List<ProductStore> productStores);
    Task UpdatePriceAsync(Guid productStoreId, decimal price);
    Task DeleteAsync(ProductStore existingProductStore);
    Task<bool> ProductStoreExistsAsync(Guid productStoreId);
}