using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Config;
using PravaCijena.Api.Database;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Middlewares;
using PravaCijena.Api.Repository;
using PravaCijena.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

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

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();

builder.Services.AddScoped<IProductStoreService, ProductStoreService>();
builder.Services.AddScoped<IProductStoreRepository, ProductStoreRepository>();

builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();

builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<ILabelRepository, LabelRepository>();

builder.Services.AddScoped<IValueService, ValueService>();
builder.Services.AddScoped<IValueRepository, ValueRepository>();

builder.Services.AddHttpClient<ICatalogueService, CatalogueService>();
builder.Services.AddScoped<ICatalogueService, CatalogueService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnableTryItOutByDefault());
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();