using PravaCijena.Api.Dto.Product;

namespace PravaCijena.Api.Models;

public class MappedProduct
{
    public required ProductWithSimilarityDto ExistingProduct { get; set; }
    public required ProductPreview ProductPreview { get; set; }
}