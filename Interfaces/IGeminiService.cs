using Part = PravaCijena.Api.Services.Gemini.GeminiRequest.Part;

namespace PravaCijena.Api.Interfaces;

public interface IGeminiService
{
    public Task<string> SendRequestAsync(List<Part> parts);
}