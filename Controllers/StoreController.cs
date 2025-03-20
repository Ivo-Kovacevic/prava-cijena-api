using api.Dto.Store;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/stores")]
public class StoreController : ControllerBase
{
    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StoreDto>>> Index()
    {
        var storesDto = await _storeService.GetStoresAsync();
        return Ok(storesDto);
    }

    [HttpGet("{storeId:guid}")]
    public async Task<ActionResult<StoreDto>> Show(Guid storeId)
    {
        var storeDto = await _storeService.GetStoreByIdAsync(storeId);
        return Ok(storeDto);
    }

    [HttpPost]
    public async Task<ActionResult<StoreDto>> Store(CreateStoreRequestDto storeRequestDto)
    {
        var storeDto = await _storeService.CreateStoreAsync(storeRequestDto);
        return CreatedAtAction(nameof(Show), new { storeId = storeDto.Id }, storeDto);
    }

    [HttpPatch("{storeId:guid}")]
    public async Task<ActionResult<StoreDto>> Update(Guid storeId, UpdateStoreRequestDto storeRequestDto)
    {
        var storeDto = await _storeService.UpdateStoreAsync(storeId, storeRequestDto);
        return Ok(storeDto);
    }

    [HttpDelete("{storeId:guid}")]
    public async Task<IActionResult> Destroy(Guid storeId)
    {
        await _storeService.DeleteStoreAsync(storeId);
        return NoContent();
    }
}