using api.Database;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Load environment variables
Env.Load();
string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
string dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
string dbUsername = Environment.GetEnvironmentVariable("DB_USERNAME");
string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    .Replace("{DB_HOST}", dbHost)
    .Replace("{DB_DATABASE}", dbDatabase)
    .Replace("{DB_USERNAME}", dbUsername)
    .Replace("{DB_PASSWORD}", dbPassword);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
