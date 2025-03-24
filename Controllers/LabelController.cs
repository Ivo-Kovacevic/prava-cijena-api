using api.Dto.Label;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api")]
public class LabelController : ControllerBase
{
    private readonly ILabelService _labelService;

    public LabelController(ILabelService labelService)
    {
        _labelService = labelService;
    }

    /*
     * SLUG ENDPOINTS
     * These endpoints are for public access because they provide readable URLs
     */
    [HttpGet("categories/{categorySlug}/labels")]
    public async Task<ActionResult<IEnumerable<LabelDto>>> IndexBySlug(string categorySlug)
    {
        var labelsDto = await _labelService.GetLabelsByCategorySlugAsync(categorySlug);
        return Ok(labelsDto);
    }

    [HttpGet("labels/{labelSlug}")]
    public async Task<ActionResult<LabelDto>> ShowBySlug(string labelSlug)
    {
        var labelDto = await _labelService.GetLabelBySlugAsync(labelSlug);
        return Ok(labelDto);
    }

    /*
     * ID ENDPOINTS
     * These endpoints are for internal use
     */
    [HttpGet("categories/{categoryId:guid}/labels")]
    public async Task<ActionResult<IEnumerable<LabelDto>>> Index(Guid categoryId)
    {
        var labelsDto = await _labelService.GetLabelsByCategoryIdAsync(categoryId);
        return Ok(labelsDto);
    }

    [HttpGet("categories/{categoryId:guid}/labels/{labelId:guid}")]
    public async Task<ActionResult<LabelDto>> Show(Guid categoryId, Guid labelId)
    {
        var labelDto = await _labelService.GetLabelByIdAsync(categoryId, labelId);
        return Ok(labelDto);
    }

    [HttpPost("categories/{categoryId:guid}/labels")]
    public async Task<ActionResult<LabelDto>> Store(Guid categoryId, CreateLabelRequestDto labelRequestDto)
    {
        var labelDto = await _labelService.CreateLabelAsync(categoryId, labelRequestDto);
        return CreatedAtAction(nameof(Show), new { categoryId, labelId = labelDto.Id }, labelDto);
    }

    [HttpPatch("categories/{categoryId:guid}/labels/{labelId:guid}")]
    public async Task<ActionResult<LabelDto>> Update(
        Guid categoryId,
        Guid labelId,
        UpdateLabelRequestDto labelRequestDto
    )
    {
        var labelDto = await _labelService.UpdateLabelAsync(categoryId, labelId, labelRequestDto);
        return Ok(labelDto);
    }

    [HttpDelete("categories/{categoryId:guid}/labels/{labelId:guid}")]
    public async Task<IActionResult> Destroy(Guid categoryId, Guid labelId)
    {
        await _labelService.DeleteLabelAsync(categoryId, labelId);
        return NoContent();
    }
}