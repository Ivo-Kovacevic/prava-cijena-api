using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("api/automation")]
public class AutomationController : ControllerBase
{
    private readonly ICatalogueService _catalogueService;
    private readonly IGeminiService _geminiService;
    private readonly IScrapingService _scrapingService;

    public AutomationController(IScrapingService scrapingService, ICatalogueService catalogueService,
        IGeminiService geminiService)
    {
        _scrapingService = scrapingService;
        _catalogueService = catalogueService;
        _geminiService = geminiService;
    }

    // [HttpPost("compare")]
    // public async Task<IActionResult> Compare(string existingProduct, string newProduct)
    // {
    //     var result = await _geminiService.CompareProductsAsync();
    //     return Ok(result);
    // }

    [HttpPost("scrape")]
    public async Task<ActionResult<AutomationResult>> Scrape()
    {
        var result = await _scrapingService.RunScraper();
        return Ok(result);
    }

    [HttpPost("analyze-catalogue")]
    public async Task<IActionResult> AnalyzePdf(IFormFile pdfFile)
    {
        var result = await _catalogueService.AnalyzePdf(pdfFile);
        return Ok(result);
    }
}