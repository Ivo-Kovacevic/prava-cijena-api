using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepo;

    public ProductController(IProductRepository productRepository)
    {
        _productRepo = productRepository;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _productRepo.GetAllAsync();
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var product = await _productRepo.GetBySlugAsync(slug);
        if (product == null)
        {
            return NotFound($"Product with slug '{slug}' not found.");
        }

        return Ok(product);
    }
}