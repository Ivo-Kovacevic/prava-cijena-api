using api.Dto.ProductStore;

namespace api.Interfaces;

public interface IProductStoreService
{
    Task<IEnumerable<ProductStoreDto>> GetStoresByProductIdAsync(Guid productId);
    Task<ProductStoreDto> GetProductStoreAsync(Guid productId, Guid storeId);
    Task<ProductStoreDto> CreateProductStoreAsync(CreateProductStoreRequestDto productStoreRequestDto);

    Task<ProductStoreDto> UpdateProductStoreAsync(
        Guid productId,
        Guid storeId,
        UpdateProductStoreRequestDto productStoreRequestDto
    );

    Task DeleteProductStoreAsync(Guid productId, Guid storeId);
}