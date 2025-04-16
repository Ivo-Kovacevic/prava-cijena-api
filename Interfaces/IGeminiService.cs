namespace PravaCijena.Api.Interfaces;

public interface IGeminiService
{
    public Task<string> CompareProductNamesAsync(string existingProduct, string newProduct);
}