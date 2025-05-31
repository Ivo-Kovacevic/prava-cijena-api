using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface ISavedProductRepository
{
    public Task<List<ProductWithMetadata>> GetAll(string userId);
    public Task<SavedProduct?> Get(string userId, Guid productId);
    public Task<Product> Create(SavedProduct savedProduct);
    public Task Delete(SavedProduct savedProduct);
}