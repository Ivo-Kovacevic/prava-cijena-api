using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<List<StoreLocation>> GetAllStoreLocations(string userId)
    {
        var storeLocation = await _cartRepository.GetAllStoreLocations(userId);

        return storeLocation;
    }

    public async Task<List<Product>> GetAllProducts(string userId)
    {
        var products = await _cartRepository.GetAllProducts(userId);

        return products;
    }

    public async Task<Product> Store(string userId, Guid productId)
    {
        var product = await _cartRepository.Create(new Cart
        {
            UserId = userId,
            ProductId = productId
        });

        return product;
    }

    public async Task<Cart?> Destroy(string userId, Guid productId)
    {
        var existingCartItem = await _cartRepository.Get(userId, productId);
        if (existingCartItem == null)
        {
            return null;
        }

        await _cartRepository.Delete(existingCartItem);
        return existingCartItem;
    }
}