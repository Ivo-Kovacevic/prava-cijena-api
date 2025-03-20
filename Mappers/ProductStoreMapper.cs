using api.Dto.ProductStore;
using api.Models;

namespace api.Mappers;

public static class ProductStoreMapper
{
    public static ProductStoreDto ToProductStoreDto(this ProductStore productStore)
    {
        return new ProductStoreDto
        {
            ProductId = productStore.ProductId,
            StoreId = productStore.StoreId
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