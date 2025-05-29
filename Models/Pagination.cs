using PravaCijena.Api.Dto.Product;

namespace PravaCijena.Api.Models;

public class Pagination
{
    public required int CurrentPage { get; set; }
    public required int TotalPages { get; set; }
    public required List<ProductWithMetadata> Products { get; set; }
}