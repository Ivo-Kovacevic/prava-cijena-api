using DotNetEnv;

namespace api.Config;

public abstract class ApiConfig
{
    protected readonly string GeminiApiKey;

    protected ApiConfig()
    {
        Env.Load();
        GeminiApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? string.Empty;

        if (string.IsNullOrEmpty(GeminiApiKey))
        {
            throw new InvalidOperationException("GEMINI_API_KEY is not set.");
        }
    }
}