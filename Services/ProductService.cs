using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Exceptions;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Mappers;

namespace PravaCijena.Api.Services;

public class ProductService : IProductService
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IProductRepository _productRepo;
    private readonly IProductStoreRepository _productStoreRepo;

    public ProductService(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
        _categoryRepo = categoryRepository;
        _productRepo = productRepository;
    }

    /*
     * SLUG SERVICE
     */
    public async Task<IEnumerable<ProductWithMetadata>> GetProductsByCategorySlugAsync(string categorySlug,
        string? userId,
        QueryObject query)
    {
        var category = await _categoryRepo.GetBySlugAsync(categorySlug);
        if (category == null)
        {
            throw new NotFoundException($"Category '{categorySlug}' not found.");
        }

        var products = await _productRepo.GetPageProductsByCategoryIdAsync(category.Id, userId, query);

        return products;
    }

    public async Task<ProductDto> GetProductBySlugAsync(string productSlug)
    {
        var product = await _productRepo.GetProductBySlugAsync(productSlug);
        if (product == null)
        {
            throw new NotFoundException($"Product '{productSlug}' not found.");
        }

        return product.ToProductDto();
    }

    public async Task<List<StoreWithPriceDto>> GetProductStoresBySlugAsync(string productSlug)
    {
        var stores = await _productRepo.GetProductStoresBySlugsAsync(productSlug);

        return stores;
    }

    public async Task<IEnumerable<ProductDto>> SearchProduct(string productName, int page, int limit)
    {
        var products = await _productRepo.Search(productName, page, limit);

        return products.Select(p => p.ToProductDto());
    }

    /*
     * ID SERVICE
     */
    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId, QueryObject query)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var products = await _productRepo.GetProductsByCategoryIdAsync(categoryId, query);

        return products.Select(p => p.ToProductDto());
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid categoryId, Guid productId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var product = await _productRepo.GetProductByIdAsync(productId);
        if (product == null)
        {
            throw new NotFoundException($"Product with id '{productId}' not found.");
        }

        return product.ToProductDto();
    }

    public async Task<ProductDto> CreateProductAsync(Guid categoryId, CreateProductRequestDto productRequestDto)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var product = productRequestDto.ProductFromCreateRequestDto(categoryId);
        product = await _productRepo.CreateAsync(product);

        return product.ToProductDto();
    }

    public async Task<ProductDto> UpdateProductAsync(
        Guid categoryId,
        Guid productId,
        UpdateProductRequestDto productRequestDto
    )
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var existingProduct = await _productRepo.GetProductByIdAsync(productId);
        if (existingProduct == null)
        {
            throw new NotFoundException($"Product with id '{productId}' not found.");
        }

        existingProduct.ToProductFromUpdateDto(productRequestDto);
        existingProduct = await _productRepo.UpdateAsync(existingProduct);

        return existingProduct.ToProductDto();
    }

    public async Task DeleteProductAsync(Guid categoryId, Guid productId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var existingProduct = await _productRepo.GetProductByIdAsync(productId);
        if (existingProduct == null)
        {
            throw new NotFoundException($"Product with id '{productId}' not found.");
        }

        await _productRepo.DeleteAsync(existingProduct);
    }
}