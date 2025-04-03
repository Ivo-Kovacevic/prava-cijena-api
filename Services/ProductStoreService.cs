using PravaCijena.Api.Dto.ProductStore;
using PravaCijena.Api.Exceptions;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Mappers;

namespace PravaCijena.Api.Services;

public class ProductStoreService : IProductStoreService
{
    private readonly IProductStoreRepository _productStoreRepo;

    public ProductStoreService(IProductStoreRepository productStoreRepository)
    {
        _productStoreRepo = productStoreRepository;
    }

    public async Task<IEnumerable<ProductStoreDto>> GetStoresByProductIdAsync(Guid productId)
    {
        var productStores = await _productStoreRepo.GetStoresByProductIdAsync(productId);

        return productStores.Select(ps => ps.ToProductStoreDto());
    }

    public async Task<ProductStoreDto> GetProductStoreAsync(Guid productId, Guid storeId)
    {
        var productStore = await _productStoreRepo.GetProductStoreByIdsAsync(productId, storeId);
        if (productStore == null)
        {
            throw new NotFoundException("Could not find product store with provided ids.");
        }

        return productStore.ToProductStoreDto();
    }

    public async Task<ProductStoreDto> CreateProductStoreAsync(CreateProductStoreRequestDto productStoreRequestDto)
    {
        var productStore = productStoreRequestDto.ToProductStoreFromCreateRequestDto();
        productStore = await _productStoreRepo.CreateAsync(productStore);

        return productStore.ToProductStoreDto();
    }

    public async Task<ProductStoreDto> UpdateProductStoreAsync(
        Guid productId,
        Guid storeId,
        UpdateProductStoreRequestDto productStoreRequestDto
    )
    {
        var existingProductStore = await _productStoreRepo.GetProductStoreByIdsAsync(productId, storeId);
        if (existingProductStore == null)
        {
            throw new NotFoundException("Could not find product store with provided ids.");
        }

        existingProductStore.ToProductStoreFromUpdateDto(productStoreRequestDto);
        existingProductStore = await _productStoreRepo.UpdateAsync(existingProductStore);

        return existingProductStore.ToProductStoreDto();
    }

    public async Task DeleteProductStoreAsync(Guid productId, Guid storeId)
    {
        var existingProductStore = await _productStoreRepo.GetProductStoreByIdsAsync(productId, storeId);
        if (existingProductStore == null)
        {
            throw new NotFoundException("Could not find product store with provided ids.");
        }

        await _productStoreRepo.DeleteAsync(existingProductStore);
    }
}