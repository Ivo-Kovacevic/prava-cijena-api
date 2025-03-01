using DotNetEnv;

namespace api.Config;

public static class DatabaseConfig
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        Env.Load(); // Load environment variables

        string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        string dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
        string dbUsername = Environment.GetEnvironmentVariable("DB_USERNAME");
        string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

        return configuration.GetConnectionString("DefaultConnection")
            .Replace("{DB_HOST}", dbHost)
            .Replace("{DB_DATABASE}", dbDatabase)
            .Replace("{DB_USERNAME}", dbUsername)
            .Replace("{DB_PASSWORD}", dbPassword);
    }
}
