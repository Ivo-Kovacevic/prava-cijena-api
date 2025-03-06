using api.DTOs;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("/api/categories/{categoryId:guid}/products")]
public class ProductController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IProductRepository _productRepo;

    public ProductController(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
        _categoryRepo = categoryRepository;
        _productRepo = productRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductDto>> Index(Guid categoryId)
    {
        return await _productRepo.GetProductsByCategoryIdAsync(categoryId);
    }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> Show(Guid categoryId, Guid productId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            return NotFound($"Category with id '{categoryId}' not found.");
        }

        var productDto = await _productRepo.GetProductByIdAsync(productId);
        if (productDto == null)
        {
            return NotFound($"Product with Id '{productId}' not found.");
        }

        return Ok(productDto);
    }

    [HttpPost]
    public async Task<IActionResult> Store(Guid categoryId, CreateProductRequestDto productRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            return NotFound($"Category with id '{categoryId}' not found.");
        }

        var product = productRequestDto.ProductFromCreateRequestDto(categoryId);

        var createdProduct = await _productRepo.CreateAsync(product);

        return CreatedAtAction(nameof(Show), new { categoryId, productId = product.Id }, createdProduct);
    }

    [HttpPut("{productId:guid}")]
    public async Task<IActionResult> Update(Guid categoryId, Guid productId, UpdateProductRequestDto productRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            return NotFound($"Category with id '{categoryId}' not found.");
        }

        var product = productRequestDto.ToProductFromUpdateDto(categoryId);

        var updatedProduct = await _productRepo.UpdateAsync(productId, product);
        if (updatedProduct == null)
        {
            return NotFound($"Product with ID '{productId}' not found.");
        }

        return Ok(updatedProduct);
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> Destroy(Guid categoryId, Guid productId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            return NotFound($"Category with id '{categoryId}' not found.");
        }

        var productDeleted = await _productRepo.DeleteAsync(productId);
        if (productDeleted == false)
        {
            return NotFound($"Product with ID '{productId}' not found.");
        }

        return NoContent();
    }
}