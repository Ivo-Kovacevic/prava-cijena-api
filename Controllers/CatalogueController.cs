using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogueController : ControllerBase
{
    private readonly CatalogueService _catalogueService;

    public CatalogueController(CatalogueService catalogueService)
    {
        _catalogueService = catalogueService;
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzePdf(string pdfPath, string prompt)
    {
        var result = await _catalogueService.ExtractDataFromPdf(pdfPath, prompt);
        return Ok(result);
    }
}