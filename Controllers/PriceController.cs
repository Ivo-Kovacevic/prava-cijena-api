using api.Dto.Price;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/product-store/{productStoreId:guid}/prices")]
public class PriceController : ControllerBase
{
    private readonly IPriceService _priceService;

    public PriceController(IPriceService priceService)
    {
        _priceService = priceService;
    }

    [HttpGet]
    public async Task<ActionResult<PriceDto>> Index(Guid productStoreId)
    {
        var pricesDto = await _priceService.GetPricesAsync(productStoreId);
        return Ok(pricesDto);
    }

    [HttpGet("{priceId:guid}")]
    public async Task<ActionResult<PriceDto>> Show(Guid productStoreId, Guid priceId)
    {
        var priceDto = await _priceService.GetPriceByIdAsync(productStoreId, priceId);
        return Ok(priceDto);
    }

    [HttpPost]
    public async Task<ActionResult<PriceDto>> Create(Guid productStoreId, CreatePriceRequestDto priceRequestDto)
    {
        var priceDto = await _priceService.CreatePriceAsync(productStoreId, priceRequestDto);
        return CreatedAtAction(nameof(Show), new { productStoreId, priceId = priceDto.Id }, priceDto);
    }
}