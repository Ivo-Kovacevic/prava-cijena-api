using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Dto.ProductStore;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("api/product-stores")]
public class ProductStoreController : ControllerBase
{
    private readonly IProductStoreService _productStoreService;

    public ProductStoreController(IProductStoreService productStoreService)
    {
        _productStoreService = productStoreService;
    }

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<IEnumerable<ProductStoreDto>>> Index(Guid productId)
    {
        var productStoresDto = await _productStoreService.GetStoresByProductIdAsync(productId);
        return Ok(productStoresDto);
    }

    [HttpGet("{productId:guid}/{storeId:guid}")]
    public async Task<ActionResult<ProductStoreDto>> Show(Guid productId, Guid storeId)
    {
        var productStoreDto = await _productStoreService.GetProductStoreAsync(productId, storeId);
        return Ok(productStoreDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductStoreDto>> Create(CreateProductStoreRequestDto productStoreRequestDto)
    {
        var productStoreDto = await _productStoreService.CreateProductStoreAsync(productStoreRequestDto);
        return CreatedAtAction(
            nameof(Show),
            new { productId = productStoreDto.ProductId, storeId = productStoreDto.StoreLocationId }, productStoreDto
        );
    }

    [HttpPatch("{productId:guid}/{storeId:guid}")]
    public async Task<ActionResult<ProductStoreDto>> Update(
        Guid productId,
        Guid storeId,
        UpdateProductStoreRequestDto productStoreRequestDto
    )
    {
        var productStoreDto = await _productStoreService.UpdateProductStoreAsync(
            productId, storeId, productStoreRequestDto
        );
        return Ok(productStoreDto);
    }

    [HttpDelete("{productId:guid}/{storeId:guid}")]
    public async Task<IActionResult> Destroy(Guid productId, Guid storeId)
    {
        await _productStoreService.DeleteProductStoreAsync(productId, storeId);
        return NoContent();
    }
}