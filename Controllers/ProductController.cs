using api.Dto.Product;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/categories/{categoryId:guid}/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Index(Guid categoryId)
    {
        var productsDto = await _productService.GetProductsByCategoryIdAsync(categoryId);
        return Ok(productsDto);
    }

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<ProductDto>> Show(Guid categoryId, Guid productId)
    {
        var productDto = await _productService.GetProductByIdAsync(categoryId, productId);
        return Ok(productDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Store(Guid categoryId, CreateProductRequestDto productRequestDto)
    {
        var productDto = await _productService.CreateProductAsync(categoryId, productRequestDto);
        return CreatedAtAction(nameof(Show), new { categoryId, productId = productDto.Id }, productDto);
    }

    [HttpPut("{productId:guid}")]
    public async Task<ActionResult<ProductDto>> Update(
        Guid categoryId,
        Guid productId,
        UpdateProductRequestDto productRequestDto
    )
    {
        var productDto = await _productService.UpdateProductAsync(categoryId, productId, productRequestDto);
        return Ok(productDto);
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> Destroy(Guid categoryId, Guid productId)
    {
        await _productService.DeleteProductAsync(categoryId, productId);
        return NoContent();
    }
}