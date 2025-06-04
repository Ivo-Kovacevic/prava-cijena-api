using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IProductStoreLinkRepository
{
    public Task<ProductStoreLink?> Get(Guid storeId, Guid productId);
    public Task<ProductStoreLink> Create(ProductStoreLink productStoreLink);
}