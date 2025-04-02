using api.Dto.Value;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("attributes/{attributeId:guid}/values")]
public class ValueController : ControllerBase
{
    private readonly IValueService _valueService;

    public ValueController(IValueService valueService)
    {
        _valueService = valueService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ValueDto>>> Index(Guid attributeId)
    {
        var attributesDto = await _valueService.GetValuesByLabelIdAsync(attributeId);
        return Ok(attributesDto);
    }

    [HttpGet("{valueId:guid}")]
    public async Task<ActionResult<ValueDto>> Show(Guid attributeId, Guid valueId)
    {
        var valueDto = await _valueService.GetValueByIdAsync(attributeId, attributeId);
        return Ok(valueDto);
    }

    [HttpPost]
    public async Task<ActionResult<ValueDto>> Store(Guid attributeId, CreateValueRequestDto attributeRequestDto)
    {
        var valueDto = await _valueService.CreateValueAsync(attributeId, attributeRequestDto);
        return CreatedAtAction(nameof(Show), new { attributeId, valueId = valueDto.Id }, valueDto);
    }

    [HttpPatch("{valueId:guid}")]
    public async Task<ActionResult<ValueDto>> Update(
        Guid attributeId,
        Guid valueId,
        UpdateValueRequestDto attributeRequestDto
    )
    {
        var valueDto = await _valueService.UpdateValueAsync(attributeId, valueId, attributeRequestDto);
        return Ok(valueDto);
    }

    [HttpDelete("{valueId:guid}")]
    public async Task<IActionResult> Destroy(Guid attributeId, Guid valueId)
    {
        await _valueService.DeleteValueAsync(attributeId, valueId);
        return NoContent();
    }
}