using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutomationController : ControllerBase
{
    private readonly ICatalogueService _catalogueService;
    private readonly IScrapingService _scrapingService;

    public AutomationController(IScrapingService scrapingService, ICatalogueService catalogueService)
    {
        _scrapingService = scrapingService;
        _catalogueService = catalogueService;
    }

    [HttpPost("scrape")]
    public async Task<IActionResult> Scrape()
    {
        var result = await _scrapingService.RunScraper();
        return Ok(result);
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzePdf(IFormFile pdfFile)
    {
        var result = await _catalogueService.ExtractDataFromPdf(pdfFile);
        return Ok(result);
    }
}