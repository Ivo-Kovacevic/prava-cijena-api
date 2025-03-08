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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StoreDto>> Show(Guid id)
    {
        var storeDto = await _storeService.GetStoreByIdAsync(id);
        return Ok(storeDto);
    }

    [HttpPost]
    public async Task<ActionResult<StoreDto>> Store(CreateStoreRequestDto storeRequestDto)
    {
        var storeDto = await _storeService.CreateStoreAsync(storeRequestDto);
        return CreatedAtAction(nameof(Show), new { id = storeDto.Id }, storeDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<StoreDto>> Update(Guid id, UpdateStoreRequestDto storeRequestDto)
    {
        var storeDto = await _storeService.UpdateStoreAsync(id, storeRequestDto);
        return Ok(storeDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Destroy(Guid id)
    {
        await _storeService.DeleteStoreAsync(id);
        return NoContent();
    }
}