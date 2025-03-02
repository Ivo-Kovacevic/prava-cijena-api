using api.DTOs;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

// GET requests use slugs
// All other requests use IDs
// /api/categories/{categoryIdOrSlug}/products/{productIdOrSlug}

[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepo;

    public ProductController(IProductRepository productRepository)
    {
        _productRepo = productRepository;
    }
    
    [HttpGet("api/categories/{categorySlug}/products")]
    public async Task<IEnumerable<Product>> GetProductsByCategory(string categorySlug)
    {
        var products = await _productRepo.GetProductsByCategoryAsync(categorySlug);
        return products;
    }

    [HttpGet("api/categories/{categorySlug}/products/{productSlug}")]
    public async Task<IActionResult> Get(string categoryIdOrSlug, string productSlug)
    {
        var product = await _productRepo.GetBySlugAsync(productSlug);
        if (product == null)
        {
            return NotFound($"Product with slug '{productSlug}' not found.");
        }

        return Ok(product);
    }

    [HttpPost("api/categories/{categoryId:int}/products")]
    public async Task<IActionResult> Create(int categoryId, CreateProductRequestDto productRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var product = productRequestDto.CreateProductFromDto(categoryId);
        await _productRepo.CreateAsync(product);

        return CreatedAtAction(nameof(Get), new { slug = product.Slug }, product);
    }
    
    [HttpPut("api/categories/{categoryId:int}/products/{productId:int}")]
    public async Task<IActionResult> Update(int categoryId, int productId, UpdateProductRequestDto productRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var updatedProduct = productRequestDto.UpdateProductFromDto(categoryId);

        updatedProduct = await _productRepo.UpdateAsync(productId, updatedProduct);
        if (updatedProduct == null)
        {
            return NotFound($"Product with id '{productId}' not found.");
        }

        return Ok(updatedProduct);
    }

    [HttpDelete("api/categories/{categoryId:int}/products/{productId:int}")]
    public async Task<IActionResult> Delete(int categoryId, int productId)
    {
        var product = await _productRepo.DeleteAsync(productId);
        if (product == null)
        {
            return NotFound($"Product with id '{productId}' not found.");
        }

        return NoContent();
    }
}