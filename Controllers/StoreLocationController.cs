using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("api/store-location")]
public class StoreLocationController : ControllerBase
{
    private readonly IStoreLocationService _storeLocationService;

    public StoreLocationController(IStoreLocationService storeLocationService)
    {
        _storeLocationService = storeLocationService;
    }

    [HttpGet("{productSlug}/{storeSlug}")]
    public async Task<IActionResult> ShowBySlug(string productSlug, string storeSlug)
    {
        var storelocationsDto = await _storeLocationService.GetStorelocationsBySlug(productSlug, storeSlug);
        return Ok(storelocationsDto);
    }

    [HttpGet("{storeId}")]
    public async Task<IActionResult> ShowBySlug(Guid storeId)
    {
        var storelocationsDto = await _storeLocationService.GetStorelocationsByStoreId(storeId);
        return Ok(storelocationsDto);
    }
}