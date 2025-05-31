using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface ICartService
{
    public Task<List<StoreLocation>> GetAllStoreLocations(string userId);
    public Task<List<Product>> GetAllProducts(string userId);
    public Task<Product> Store(string userId, Guid productId);
    public Task<Cart?> Destroy(string userId, Guid productId);
}