using DotNetEnv;

namespace api.Config;

public static class DatabaseConfig
{
    public static string GetConnectionString()
    {
        Env.Load();

        var dbHost = GetRequiredEnvVar("DB_HOST");
        var dbDatabase = GetRequiredEnvVar("DB_DATABASE");
        var dbUsername = GetRequiredEnvVar("DB_USERNAME");
        var dbPassword = GetRequiredEnvVar("DB_PASSWORD");

        return $"Host={dbHost};Database={dbDatabase};Username={dbUsername};Password={dbPassword};";
    }

    private static string GetRequiredEnvVar(string key)
    {
        var value = Environment.GetEnvironmentVariable(key) ?? string.Empty;
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Environment variable '{key}' is not set.");
        }

        return value;
    }
}