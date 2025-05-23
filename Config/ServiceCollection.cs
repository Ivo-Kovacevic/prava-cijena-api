using PravaCijena.Api.Context;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Repository;
using PravaCijena.Api.Services;
using PravaCijena.Api.Services.AutomationServices;
using PravaCijena.Api.Services.Gemini;

namespace PravaCijena.Api.Config;

public static class ServiceCollection
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IStoreRepository, StoreRepository>();

        services.AddScoped<IStoreLocationService, StoreLocationService>();
        services.AddScoped<IStoreLocationRepository, StoreLocationRepository>();

        services.AddScoped<IProductStoreService, ProductStoreService>();
        services.AddScoped<IProductStoreRepository, ProductStoreRepository>();

        services.AddScoped<IPriceService, PriceService>();
        services.AddScoped<IPriceRepository, PriceRepository>();

        services.AddScoped<ILabelService, LabelService>();
        services.AddScoped<ILabelRepository, LabelRepository>();

        services.AddScoped<IValueService, ValueService>();
        services.AddScoped<IValueRepository, ValueRepository>();

        services.AddHttpClient<IAutomationService, AutomationService>();
        services.AddScoped<IAutomationService, AutomationService>();

        services.AddHttpClient<ICatalogueService, CatalogueService>();
        services.AddScoped<ICatalogueService, CatalogueService>();

        services.AddHttpClient<IScrapingService, ScrapingService>();
        services.AddScoped<IScrapingService, ScrapingService>();

        services.AddHttpClient<IStructuredDataService, StructuredDataService>();
        services.AddScoped<IStructuredDataService, StructuredDataService>();

        services.AddHttpClient<IGeminiService, GeminiService>();
        services.AddScoped<IGeminiService, GeminiService>();

        services.AddScoped<ProductProcessingContext>();

        return services;
    }
}