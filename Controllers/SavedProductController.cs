using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("/api/saved-products")]
public class SavedProductController : ControllerBase
{
    private readonly ISavedProductService _savedProductService;

    public SavedProductController(ISavedProductService savedProductService)
    {
        _savedProductService = savedProductService;
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

        var savedProducts = await _savedProductService.GetAll(userId);
        return Ok(savedProducts);
    }

    [HttpPost("{productId:guid}")]
    [Authorize]
    public async Task<IActionResult> Store(Guid productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var savedProduct = await _savedProductService.Store(userId, productId);
        return Ok(savedProduct);
    }

    [HttpDelete("{productId:guid}")]
    [Authorize]
    public async Task<IActionResult> Destroy(Guid productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var existingSavedProduct = await _savedProductService.Destroy(userId, productId);
        if (existingSavedProduct == null)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}