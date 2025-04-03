using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Dto.Price;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
public class PriceController : ControllerBase
{
    private readonly IPriceService _priceService;

    public PriceController(IPriceService priceService)
    {
        _priceService = priceService;
    }
    
    /*
     * ID ENDPOINTS
     * These endpoints are for internal use
     */
    [HttpGet("api/product-stores/{productStoreId:guid}/prices")]
    public async Task<ActionResult<PriceDto>> Index(Guid productStoreId)
    {
        var pricesDto = await _priceService.GetPricesAsync(productStoreId);
        return Ok(pricesDto);
    }

    [HttpGet("api/product-stores/{productStoreId:guid}/prices/{priceId:guid}")]
    public async Task<ActionResult<PriceDto>> Show(Guid productStoreId, Guid priceId)
    {
        var priceDto = await _priceService.GetPriceByIdAsync(productStoreId, priceId);
        return Ok(priceDto);
    }

    [HttpPost("api/product-stores/{productStoreId:guid}/prices")]
    public async Task<ActionResult<PriceDto>> Create(Guid productStoreId, CreatePriceRequestDto priceRequestDto)
    {
        var priceDto = await _priceService.CreatePriceAsync(productStoreId, priceRequestDto);
        return CreatedAtAction(nameof(Show), new { productStoreId, priceId = priceDto.Id }, priceDto);
    }
}