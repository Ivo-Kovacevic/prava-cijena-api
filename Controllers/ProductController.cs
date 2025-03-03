using api.DTOs;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repository;
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
    public async Task<IEnumerable<ProductDto>> GetProductsByCategory(Guid categoryId)
    {
        return await _productRepo.GetProductsByCategoryIdAsync(categoryId);
    }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> Get(Guid categoryId, Guid productId)
    {
        var product = await _productRepo.GetProductByIdAsync(productId);
        if (product == null)
        {
            return NotFound($"Product with Id '{productId}' not found.");
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid categoryId, CreateProductRequestDto productRequestDto)
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

        return CreatedAtAction(nameof(Get), new { categoryId = categoryId, productId = product.Id }, createdProduct);
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
    public async Task<IActionResult> Delete(Guid categoryId, Guid productId)
    {
        var product = await _productRepo.DeleteAsync(productId);
        if (product == null)
        {
            return NotFound($"Product with ID '{productId}' not found.");
        }

        return NoContent();
    }
}