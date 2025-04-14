using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("api/")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /*
     * SLUG ENDPOINTS
     * These endpoints are for public access because they provide readable URLs
     */
    [HttpGet("categories/{categorySlug}/products")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> IndexBySlug(
        string categorySlug,
        [FromQuery] QueryObject query
    )
    {
        var productsDto = await _productService.GetProductsByCategorySlugAsync(categorySlug, query);
        return Ok(productsDto);
    }

    [HttpGet("products/{productSlug}")]
    public async Task<ActionResult<ProductDto>> ShowBySlug(string productSlug)
    {
        var productDto = await _productService.GetProductBySlugAsync(productSlug);
        return Ok(productDto);
    }

    [HttpGet("products/search")]
    public async Task<ActionResult<IEnumerable<Product>>> Search([FromQuery] string searchTerm)
    {
        var productsDto = await _productService.SearchProduct(searchTerm);
        return Ok(productsDto);
    }

    /*
     * ID ENDPOINTS
     * These endpoints are for internal use
     */
    [HttpGet("categories/{categoryId:guid}/products")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Index(Guid categoryId, [FromQuery] QueryObject query)
    {
        var productsDto = await _productService.GetProductsByCategoryIdAsync(categoryId, query);
        return Ok(productsDto);
    }

    [HttpGet("categories/{categoryId:guid}/products/{productId:guid}")]
    public async Task<ActionResult<ProductDto>> Show(Guid categoryId, Guid productId)
    {
        var productDto = await _productService.GetProductByIdAsync(categoryId, productId);
        return Ok(productDto);
    }

    [HttpPost("categories/{categoryId:guid}/products")]
    public async Task<ActionResult<ProductDto>> Store(Guid categoryId, CreateProductRequestDto productRequestDto)
    {
        var productDto = await _productService.CreateProductAsync(categoryId, productRequestDto);
        return CreatedAtAction(nameof(Show), new { categoryId, productId = productDto.Id }, productDto);
    }

    [HttpPatch("categories/{categoryId:guid}/products/{productId:guid}")]
    public async Task<ActionResult<ProductDto>> Update(
        Guid categoryId,
        Guid productId,
        UpdateProductRequestDto productRequestDto
    )
    {
        var productDto = await _productService.UpdateProductAsync(categoryId, productId, productRequestDto);
        return Ok(productDto);
    }

    [HttpDelete("categories/{categoryId:guid}/products/{productId:guid}")]
    public async Task<IActionResult> Destroy(Guid categoryId, Guid productId)
    {
        await _productService.DeleteProductAsync(categoryId, productId);
        return NoContent();
    }
}