using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface ISavedProductService
{
    public Task<List<ProductWithMetadata>> GetAll(string userId);
    public Task<Product> Store(string userId, Guid productId);
    public Task<SavedProduct?> Destroy(string userId, Guid productId);
}