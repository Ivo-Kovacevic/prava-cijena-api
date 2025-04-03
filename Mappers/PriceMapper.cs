using PravaCijena.Api.Dto.Price;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Mappers;

public static class PriceMapper
{
    public static PriceDto ToPriceDto(this Price price)
    {
        return new PriceDto
        {
            Amount = price.Amount,
            ProductStoreId = price.ProductStoreId
        };
    }

    public static Price PriceFromCreateRequestDto(this CreatePriceRequestDto priceRequestDto, Guid productStoreId)
    {
        return new Price
        {
            Amount = priceRequestDto.Amount,
            ProductStoreId = productStoreId
        };
    }

    public static void ToPriceFromUpdateDto(
        this Price existingPrice,
        UpdatePriceRequestDto priceRequestDto
    )
    {
        existingPrice.Amount = priceRequestDto.Amount ?? existingPrice.Amount;
        existingPrice.ProductStoreId = priceRequestDto.ProductStoreId ?? existingPrice.ProductStoreId;
    }
}