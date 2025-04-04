using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Config;
using PravaCijena.Api.Database;
using PravaCijena.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Connect to database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(DatabaseConfig.GetConnectionString())
);

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAppServices();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnableTryItOutByDefault());
}
else
{
    app.UseHttpsRedirection();
}

app.UseExceptionHandler();
app.MapControllers();

// Add Postgre Trigrams extension that provides searching for products functionality
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.ExecuteSqlRaw("CREATE EXTENSION IF NOT EXISTS pg_trgm;");
}

app.Run();