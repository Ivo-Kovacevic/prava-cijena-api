using System.ComponentModel.DataAnnotations;

namespace api.Dto.ProductStore;

public class UpdateProductStoreRequestDto
{
    public Guid? ProductId { get; set; }
    public Guid? StoreId { get; set; }

    [Url(ErrorMessage = "Product URL must be in URL format")]
    public string? ProductUrl { get; set; }

    public decimal? LatestPrice { get; set; }
}