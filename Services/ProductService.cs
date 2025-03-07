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
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var productDto = await _productRepo.GetProductByIdAsync(productId);
        if (productDto == null)
        {
            throw new NotFoundException($"Product with id '{productId}' not found.");
        }

        return productDto;
    }

    public async Task<ProductDto> CreateProductAsync(Guid categoryId, CreateProductRequestDto productRequestDto)
    {
        var product = productRequestDto.ProductFromCreateRequestDto(categoryId);

        return await _productRepo.CreateAsync(product);
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

        var product = productRequestDto.ToProductFromUpdateDto(categoryId);

        return await _productRepo.UpdateAsync(productId, product);
    }

    public async Task DeleteProductAsync(Guid categoryId, Guid productId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        await _productRepo.DeleteAsync(productId);
    }
}