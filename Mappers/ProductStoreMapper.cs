using PravaCijena.Api.Dto.ProductStore;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Mappers;

public static class ProductStoreMapper
{
    public static ProductStoreDto ToProductStoreDto(this ProductStore productStore)
    {
        return new ProductStoreDto
        {
            Id = productStore.Id,
            ProductId = productStore.ProductId,
            StoreId = productStore.StoreId,
            LatestPrice = productStore.LatestPrice,
            ProductUrl = productStore.ProductUrl
        };
    }

    public static ProductStore ToProductStoreFromCreateRequestDto(
        this CreateProductStoreRequestDto productStoreRequestDto)
    {
        return new ProductStore
        {
            ProductId = productStoreRequestDto.ProductId,
            StoreId = productStoreRequestDto.StoreId
        };
    }

    public static void ToProductStoreFromUpdateDto(
        this ProductStore existingProductStore,
        UpdateProductStoreRequestDto productStoreRequestDto
    )
    {
        existingProductStore.ProductId = productStoreRequestDto.ProductId ?? existingProductStore.ProductId;
        existingProductStore.StoreId = productStoreRequestDto.StoreId ?? existingProductStore.StoreId;
        existingProductStore.LatestPrice = productStoreRequestDto.LatestPrice ?? existingProductStore.LatestPrice;
        existingProductStore.ProductUrl = productStoreRequestDto.ProductUrl ?? existingProductStore.ProductUrl;
    }
}