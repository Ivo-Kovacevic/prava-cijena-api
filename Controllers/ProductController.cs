using api.Dto.Product;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("/api/categories/{categoryId:guid}/products")]
public class ProductController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public ProductController(ICategoryService categoryService, IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Index(Guid categoryId)
    {
        var products = await _productService.GetProductsByCategoryIdAsync(categoryId);
        return Ok(products);
    }

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<ProductDto>> Show(Guid categoryId, Guid productId)
    {
        var product = await _productService.GetProductByIdAsync(categoryId, productId);
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Store(Guid categoryId, CreateProductRequestDto productRequestDto)
    {
        var createdProduct = await _productService.CreateProductAsync(categoryId, productRequestDto);
        return CreatedAtAction(nameof(Show), new { categoryId, productId = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{productId:guid}")]
    public async Task<ActionResult<ProductDto>> Update(Guid categoryId, Guid productId, UpdateProductRequestDto productRequestDto)
    {
        var updatedProduct = await _productService.UpdateProductAsync(categoryId, productId, productRequestDto);
        return Ok(updatedProduct);
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> Destroy(Guid categoryId, Guid productId)
    {
        await _productService.DeleteProductAsync(categoryId, productId);
        return NoContent();
    }
}