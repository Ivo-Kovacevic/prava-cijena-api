namespace api.Dto.Price;

public class UpdatePriceRequestDto
{
    public decimal? Amount { get; set; }
    public Guid? ProductStoreId { get; set; }
}