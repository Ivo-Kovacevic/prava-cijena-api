using api.Dto.Product;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;

namespace api.Services;

public class ProductService : IProductService
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IProductRepository _productRepo;

    public ProductService(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
        _categoryRepo = categoryRepository;
        _productRepo = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        return await _productRepo.GetProductsByCategoryIdAsync(categoryId);
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid categoryId, Guid productId)
    {
        var product = await _productRepo.GetProductByIdAsync(productId);
        if (product.CategoryId != categoryId)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        if (product.Id != productId)
        {
            throw new NotFoundException($"Product with id '{productId}' not found.");
        }

        return product;
    }

    public async Task<ProductDto> CreateProductAsync(Guid categoryId, CreateProductRequestDto productRequestDto)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var product = productRequestDto.ProductFromCreateRequestDto(categoryId);

        var productDto = await _productRepo.CreateAsync(product);

        return productDto;
    }

    public async Task<ProductDto> UpdateProductAsync(Guid categoryId, Guid productId, UpdateProductRequestDto productRequestDto)
    {
        var existingProduct = await _productRepo.GetProductByIdAsync(productId);
        if (existingProduct.CategoryId != categoryId)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        if (existingProduct.Id != productId)
        {
            throw new NotFoundException($"Product with id '{productId}' not found.");
        }

        var product = productRequestDto.ToProductFromUpdateDto(categoryId);

        var productDto = await _productRepo.UpdateAsync(productId, product);

        return productDto;
    }

    public async Task DeleteProductAsync(Guid categoryId, Guid productId)
    {
        var product = await _productRepo.GetProductByIdAsync(productId);
        if (product.CategoryId != categoryId)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        if (product.Id != productId)
        {
            throw new NotFoundException($"Product with id '{productId}' not found.");
        }

        await _productRepo.DeleteAsync(productId);
    }
}