using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("api/automation")]
public class AutomationController : ControllerBase
{
    private readonly ICatalogueService _catalogueService;
    private readonly IGeminiService _geminiService;
    private readonly IScrapingService _scrapingService;
    private readonly IStructuredDataService _structuredDataService;

    public AutomationController(
        IScrapingService scrapingService,
        ICatalogueService catalogueService,
        IGeminiService geminiService,
        IStructuredDataService structuredDataService
    )
    {
        _scrapingService = scrapingService;
        _catalogueService = catalogueService;
        _geminiService = geminiService;
        _structuredDataService = structuredDataService;
    }

    [HttpPost("scrape")]
    public async Task<IActionResult> Scrape()
    {
        await _scrapingService.RunScraper();
        return Ok();
    }

    [HttpPost("assign-images")]
    public async Task<IActionResult> AssignImages()
    {
        await _scrapingService.AssignImages();
        return Ok();
    }

    [HttpPost("analyze-catalogue")]
    public async Task<IActionResult> AnalyzePdf(IFormFile? pdfFile)
    {
        var result = await _catalogueService.AnalyzePdfs();
        return Ok(result);
    }

    [HttpPost("process-structured-data")]
    public async Task<IActionResult> ProcessStructuredData()
    {
        await _structuredDataService.SyncStoreFiles();
        return Ok();
    }
}