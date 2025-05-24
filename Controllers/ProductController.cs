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
     * SLUG ENDPOINTS FOR FRONTEND
     * These endpoints are for public access because they provide readable URLs
     */

    // ENDPOINT FOR GETTING ALL PRODUCTS IN CATEGORY
    [HttpGet("categories/{categorySlug}/products")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> IndexBySlug(
        string categorySlug,
        [FromQuery] QueryObject query
    )
    {
        var productsDto = await _productService.GetProductsByCategorySlugAsync(categorySlug, query);
        return Ok(productsDto);
    }

    // ENDPOINT FOR GETTING BASIC PRODUCT DATA
    [HttpGet("products/{productSlug}")]
    public async Task<IActionResult> ProductBySlug(string productSlug)
    {
        var productDto = await _productService.GetProductBySlugAsync(productSlug);
        return Ok(productDto);
    }

    // ENDPOINT FOR GETTING LIST OF STORES WITH LOWEST PRICE FROM THEIR LOCATIONS
    [HttpGet("products/{productSlug}/product-stores")]
    public async Task<IActionResult> StoresBySlug(string productSlug)
    {
        var productDto = await _productService.GetProductStoresBySlugAsync(productSlug);
        return Ok(productDto);
    }

    // ENDPOINT FOR SEARCHING PRODUCTS
    [HttpGet("products/search")]
    public async Task<IActionResult> Search(
        [FromQuery] string searchTerm,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 5
    )
    {
        var productsDto = await _productService.SearchProduct(searchTerm, page, limit);
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