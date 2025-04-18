using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiResponse;

namespace PravaCijena.Api.Interfaces;

public interface IGeminiService
{
    public Task<ComparedResult> CompareProductsAsync(MappedProduct mappedProducts);
}