using System.Text.Json;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiResponse;
using Part = PravaCijena.Api.Services.Gemini.GeminiRequest.Part;

namespace PravaCijena.Api.Services.AutomationServices;

public class AutomationService : IAutomationService
{
    private readonly IGeminiService _geminiService;
    private readonly IPriceRepository _priceRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductStoreRepository _productStoreRepository;

    public AutomationService(
        HttpClient httpClient,
        IGeminiService geminiService,
        IProductRepository productRepository,
        IProductStoreRepository productStoreRepository,
        IPriceRepository priceRepository
    )
    {
        _geminiService = geminiService;
        _productRepository = productRepository;
        _productStoreRepository = productStoreRepository;
        _priceRepository = priceRepository;
    }

    public async Task<AutomationResult> HandleFoundProducts(
        List<ProductPreview> productPreviews,
        StoreWithCategoriesDto store,
        Guid? equivalentCategoryId,
        AutomationResult results
    )
    {
        var semaphore = new SemaphoreSlim(100);
        var mappedProducts = await MapFoundToExistingProducts(productPreviews);

        var tasks = mappedProducts.Select(async mappedProduct =>
        {
            await semaphore.WaitAsync();
            try
            {
                var response = await _geminiService.SendRequestAsync([
                    new Part
                    {
                        Text = $@"You are an AI specializing in grocery product name matching.
                                  Input: {JsonSerializer.Serialize(mappedProduct)}

                                  Each object has:
                                  - existingProduct: The product already in our database.
                                  - productPreview: A newly scraped product to compare.

                                  ### TASK:
                                  Determine if the *existingProduct.name* and *productPreview.name* are same products, considering brand, variant, and *exact* size/quantity.
                                  Ignore minor differences in capitalization, punctuation, word order, and common abbreviations (g/G, l/L).

                                  ### OUTPUT RULES:
                                  - Output MUST be a valid JSON object, not array, just single object.
                                  - Each object must have:
                                    - existingProduct (unchanged from input)
                                    - productPreview (unchanged from input)
                                    - isSameProduct (true/false)
                                  - Do NOT change or truncate any fields.
                                  - Do NOT include comments or explanations.
                                  - Do NOT output anything except the JSON.

                                  ### EXAMPLE OUTPUT:
                                  {{
                                     ""existingProduct"": {{ ... }},
                                     ""productPreview"": {{ ... }},
                                     ""isSameProduct"": true
                                  }}
                                  "
                    }
                ]);

                var result = JsonSerializer.Deserialize<ComparedResult>(response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }).ToList();

        var comparedProducts = (await Task.WhenAll(tasks)).ToList();
        await HandleComparedProducts(comparedProducts, store, equivalentCategoryId, results);

        return results;
    }

    private async Task<List<MappedProduct>> MapFoundToExistingProducts(List<ProductPreview> productPreviews)
    {
        var mappedProducts = new List<MappedProduct>();
        foreach (var productPreview in productPreviews)
        {
            var existingProduct = (await _productRepository.Search(productPreview.Name)).FirstOrDefault();
            if (existingProduct != null)
            {
                mappedProducts.Add(
                    new MappedProduct
                    {
                        ExistingProduct = existingProduct,
                        ProductPreview = productPreview
                    }
                );
            }
        }

        return mappedProducts;
    }

    private async Task HandleComparedProducts(
        List<ComparedResult> comparedResults,
        StoreWithCategoriesDto store,
        Guid? equivalentCategoryId,
        AutomationResult results
    )
    {
        foreach (var comparedResult in comparedResults)
            try
            {
                if (comparedResult.ExistingProduct.Name == "Zott Belriso Mliječni desert s rižom razni okusi 200 g" ||
                    comparedResult.ProductPreview.Name == "Zott Belriso Mliječni desert s rižom razni okusi 200 g"
                   )
                {
                    Console.WriteLine("FFS");
                }

                /*
                 * Update product if it's similar enough
                 */
                if (comparedResult.ExistingProduct.Similarity >= 0.95 || comparedResult.IsSameProduct)
                {
                    /*
                     * Update the lowest price if current price is from yesterday or if it's lower than current price
                     */
                    if (comparedResult.ExistingProduct.UpdatedAt.Date < DateTime.UtcNow.Date ||
                        comparedResult.ProductPreview.Price < comparedResult.ExistingProduct.LowestPrice
                       )
                    {
                        await _productRepository.UpdateLowestPriceAsync(
                            comparedResult.ExistingProduct.Id,
                            comparedResult.ProductPreview.Price
                        );
                        results.UpdatedProducts++;
                    }

                    var productStore = await _productStoreRepository.GetProductStoreByIdsAsync(
                        comparedResult.ExistingProduct.Id,
                        store.Id
                    );

                    if (productStore == null)
                    {
                        productStore = await _productStoreRepository.CreateAsync(new ProductStore
                        {
                            ProductId = comparedResult.ExistingProduct.Id,
                            StoreId = store.Id,
                            ProductUrl = comparedResult.ProductPreview.ProductUrl,
                            LatestPrice = comparedResult.ProductPreview.Price
                        });
                        results.CreatedProductStores++;

                        await _priceRepository.CreateAsync(new Price
                        {
                            Amount = comparedResult.ProductPreview.Price,
                            ProductStoreId = productStore.Id
                        });
                        results.CreatedPrices++;

                        continue;
                    }

                    await _productStoreRepository.UpdatePriceAsync(
                        productStore.Id,
                        comparedResult.ProductPreview.Price
                    );
                    results.UpdatedProductStores++;

                    var latestPrice = (await _priceRepository.GetPricesByProductStoreIdAsync(productStore.Id))
                        .FirstOrDefault();
                    if (latestPrice == null || latestPrice.CreatedAt.Date != DateTime.UtcNow.Date)
                    {
                        await _priceRepository.CreateAsync(new Price
                        {
                            Amount = comparedResult.ProductPreview.Price,
                            ProductStoreId = productStore.Id
                        });
                        results.CreatedPrices++;
                    }

                    continue;
                }

                /*
                 * Create new entries for each product that doesn't exist
                 */
                if (comparedResult.ExistingProduct.Similarity < 0.95
                    && !comparedResult.IsSameProduct
                    && equivalentCategoryId.HasValue
                   )
                {
                    var newProduct = await _productRepository.CreateAsync(new Product
                    {
                        Name = comparedResult.ProductPreview.Name,
                        ImageUrl = comparedResult.ProductPreview.ImageUrl,
                        CategoryId = equivalentCategoryId.Value,
                        LowestPrice = comparedResult.ProductPreview.Price
                    });
                    results.CreatedProducts++;

                    var productStore = await _productStoreRepository.CreateAsync(new ProductStore
                    {
                        ProductId = newProduct.Id,
                        StoreId = store.Id,
                        ProductUrl = comparedResult.ProductPreview.ProductUrl,
                        LatestPrice = comparedResult.ProductPreview.Price
                    });
                    results.CreatedProductStores++;

                    await _priceRepository.CreateAsync(new Price
                    {
                        Amount = comparedResult.ProductPreview.Price,
                        ProductStoreId = productStore.Id
                    });
                    results.CreatedPrices++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }
}