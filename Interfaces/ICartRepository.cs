using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface ICartRepository
{
    public Task<List<StoreLocation>> GetAllStoreLocations(string userId);
    public Task<List<Product>> GetAllProducts(string userId);
    public Task<Cart?> Get(string userId, Guid productId);
    public Task<Product> Create(Cart cart);
    public Task Delete(Cart cart);
}