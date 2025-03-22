using api.Dto.Label;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/categories/{categoryId:guid}/attributes")]
public class LabelController : ControllerBase
{
    private readonly ILabelService _labelService;

    public LabelController(ILabelService labelService)
    {
        _labelService = labelService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LabelDto>>> Index(Guid categoryId)
    {
        var attributesDto = await _labelService.GetLabelsByCategoryIdAsync(categoryId);
        return Ok(attributesDto);
    }

    [HttpGet("{attributeId:guid}")]
    public async Task<ActionResult<LabelDto>> Show(Guid categoryId, Guid attributeId)
    {
        var attributeDto = await _labelService.GetLabelByIdAsync(categoryId, attributeId);
        return Ok(attributeDto);
    }

    [HttpPost]
    public async Task<ActionResult<LabelDto>> Store(Guid categoryId, CreateLabelRequestDto labelRequestDto)
    {
        var attributeDto = await _labelService.CreateLabelAsync(categoryId, labelRequestDto);
        return CreatedAtAction(nameof(Show), new { categoryId, attributeId = attributeDto.Id }, attributeDto);
    }

    [HttpPatch("{attributeId:guid}")]
    public async Task<ActionResult<LabelDto>> Update(
        Guid categoryId,
        Guid attributeId,
        UpdateLabelRequestDto labelRequestDto
    )
    {
        var attributeDto = await _labelService.UpdateLabelAsync(categoryId, attributeId, labelRequestDto);
        return Ok(attributeDto);
    }

    [HttpDelete("{attributeId:guid}")]
    public async Task<IActionResult> Destroy(Guid categoryId, Guid attributeId)
    {
        await _labelService.DeleteLabelAsync(categoryId, attributeId);
        return NoContent();
    }
}