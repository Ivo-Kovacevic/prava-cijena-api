using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }


    // Get list of user saved stores with products
    [HttpGet]
    [Authorize]
    [Route("store-locations")]
    public async Task<IActionResult> IndexStoreLocations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var storeLocations = await _cartService.GetAllStoreLocations(userId);
        return Ok(storeLocations);
    }

    // Get list of user products in cart
    [HttpGet]
    [Authorize]
    [Route("products")]
    public async Task<IActionResult> IndexProducts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var cartItems = await _cartService.GetAllProducts(userId);
        return Ok(cartItems);
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

        var product = await _cartService.Store(userId, productId);
        return Ok(product);
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

        var existingCartItem = await _cartService.Destroy(userId, productId);
        if (existingCartItem == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}