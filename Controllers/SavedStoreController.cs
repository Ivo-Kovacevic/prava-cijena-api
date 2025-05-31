using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("/api/saved-stores")]
public class SavedStoreController : ControllerBase
{
    private readonly ISavedStoreService _savedStoreService;

    public SavedStoreController(ISavedStoreService savedStoreService)
    {
        _savedStoreService = savedStoreService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var savedStores = await _savedStoreService.GetAll(userId);
        return Ok(savedStores);
    }

    [HttpPost("{storeLocationId:guid}")]
    [Authorize]
    public async Task<IActionResult> Store(Guid storeLocationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var storeLocation = await _savedStoreService.Store(userId, storeLocationId);
        return Ok(storeLocation);
    }

    [HttpDelete("{storeLocationId:guid}")]
    [Authorize]
    public async Task<IActionResult> Destroy(Guid storeLocationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var existingSavedStore = await _savedStoreService.Destroy(userId, storeLocationId);
        if (existingSavedStore == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}