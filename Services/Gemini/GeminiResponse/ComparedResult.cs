using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Services.Gemini.GeminiResponse;

public class ComparedResult
{
    public required ProductWithSimilarityDto ExistingProduct { get; set; }
    public required ProductPreview ProductPreview { get; set; }
    public required bool IsSameProduct { get; set; }
}