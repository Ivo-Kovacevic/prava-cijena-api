using PravaCijena.Api.Dto.Price;

namespace PravaCijena.Api.Interfaces;

public interface IPriceService
{
    Task<IEnumerable<PriceDto>> GetPricesAsync(Guid productStoreId);
    Task<PriceDto> GetPriceByIdAsync(Guid productStoreId, Guid priceId);
    Task<PriceDto> CreatePriceAsync(Guid productStoreId, CreatePriceRequestDto priceRequestDto);
    Task<PriceDto> UpdatePriceAsync(Guid productStoreId, Guid priceId, UpdatePriceRequestDto priceRequestDto);
    Task DeletePriceAsync(Guid productStoreId, Guid priceId);
}