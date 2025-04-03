using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogueController : ControllerBase
{
    private readonly ICatalogueService _catalogueService;

    public CatalogueController(ICatalogueService catalogueService)
    {
        _catalogueService = catalogueService;
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzePdf(IFormFile pdfFile)
    {
        var result = await _catalogueService.ExtractDataFromPdf(pdfFile);
        return Ok(result);
    }
}