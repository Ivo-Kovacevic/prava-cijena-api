using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Price;

public class PriceDto : BaseEntity
{
    public required decimal Amount { get; set; }
    public required Guid ProductStoreId { get; set; }
}