using api.Models;

namespace api.Interfaces;

public interface IPriceRepository
{
    Task<IEnumerable<Price>> GetPricesByProductStoreIdAsync(Guid productStoreId);
    Task<Price?> GetPriceByIdAsync(Guid priceId);
    Task<Price> CreateAsync(Price price);
    Task<Price> UpdateAsync(Price existingPrice);
    Task DeleteAsync(Price existingPrice);
}