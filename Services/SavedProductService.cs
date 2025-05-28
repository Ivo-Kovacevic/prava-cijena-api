using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Exceptions;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Services;

public class SavedProductService : ISavedProductService
{
    private readonly ISavedProductRepository _savedProductRepository;

    public SavedProductService(ISavedProductRepository savedProductRepository)
    {
        _savedProductRepository = savedProductRepository;
    }

    public async Task<List<ProductWithMetadata>> GetAll(string userId)
    {
        var savedProducts = await _savedProductRepository.GetAll(userId);

        return savedProducts;
    }

    public async Task<SavedProduct> Store(string userId, Guid productId)
    {
        var savedProduct = await _savedProductRepository.Create(new SavedProduct
        {
            UserId = userId,
            ProductId = productId
        });

        return savedProduct;
    }

    public async Task Destroy(string userId, Guid productId)
    {
        var existingSavedProduct = await _savedProductRepository.Get(userId, productId);
        if (existingSavedProduct == null)
        {
            throw new NotFoundException($"SavedProduct with id '{existingSavedProduct}' not found.");
        }

        await _savedProductRepository.Delete(existingSavedProduct);
    }
}