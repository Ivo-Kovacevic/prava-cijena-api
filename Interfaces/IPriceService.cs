using api.Dto.Price;

namespace api.Interfaces;

public interface IPriceService
{
    Task<IEnumerable<PriceDto>> GetPricesAsync(Guid productStoreId);
    Task<PriceDto> GetPriceByIdAsync(Guid productStoreId, Guid priceId);
    Task<PriceDto> CreatePriceAsync(Guid productStoreId, CreatePriceRequestDto priceRequestDto);
    Task<PriceDto> UpdatePriceAsync(Guid productStoreId, UpdatePriceRequestDto priceRequestDto);
    Task DeletePriceAsync(Guid priceId);
}