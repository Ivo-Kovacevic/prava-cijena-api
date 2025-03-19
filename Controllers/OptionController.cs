using api.Dto.Option;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("attributes/{attributeId:guid}/options")]
public class OptionController : ControllerBase
{
    private readonly IOptionService _optionService;

    public OptionController(IOptionService optionService)
    {
        _optionService = optionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OptionDto>>> Index(Guid attributeId)
    {
        var attributesDto = await _optionService.GetOptionsByAttributeIdAsync(attributeId);
        return Ok(attributesDto);
    }

    [HttpGet("{optionId:guid}")]
    public async Task<ActionResult<OptionDto>> Show(Guid attributeId, Guid optionId)
    {
        var optionDto = await _optionService.GetOptionByIdAsync(attributeId, attributeId);
        return Ok(optionDto);
    }

    [HttpPost]
    public async Task<ActionResult<OptionDto>> Store(Guid attributeId, CreateOptionRequestDto attributeRequestDto)
    {
        var optionDto = await _optionService.CreateOptionAsync(attributeId, attributeRequestDto);
        return CreatedAtAction(nameof(Show), new { attributeId, optionId = optionDto.Id }, optionDto);
    }

    [HttpPut("{optionId:guid}")]
    public async Task<ActionResult<OptionDto>> Update(
        Guid attributeId,
        Guid optionId,
        UpdateOptionRequestDto attributeRequestDto
    )
    {
        var optionDto = await _optionService.UpdateOptionAsync(attributeId, optionId, attributeRequestDto);
        return Ok(optionDto);
    }

    [HttpDelete("{attributeId:guid}")]
    public async Task<IActionResult> Destroy(Guid attributeId, Guid optionId)
    {
        await _optionService.DeleteOptionAsync(attributeId, attributeId);
        return NoContent();
    }
}