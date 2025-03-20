using api.Dto.Attribute;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/categories/{categoryId:guid}/attributes")]
public class AttributeController : ControllerBase
{
    private readonly IAttributeService _attributeService;

    public AttributeController(IAttributeService attributeService)
    {
        _attributeService = attributeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttributeDto>>> Index(Guid categoryId)
    {
        var attributesDto = await _attributeService.GetAttributesByCategoryIdAsync(categoryId);
        return Ok(attributesDto);
    }

    [HttpGet("{attributeId:guid}")]
    public async Task<ActionResult<AttributeDto>> Show(Guid categoryId, Guid attributeId)
    {
        var attributeDto = await _attributeService.GetAttributeByIdAsync(categoryId, attributeId);
        return Ok(attributeDto);
    }

    [HttpPost]
    public async Task<ActionResult<AttributeDto>> Store(Guid categoryId, CreateAttributeRequestDto attributeRequestDto)
    {
        var attributeDto = await _attributeService.CreateAttributeAsync(categoryId, attributeRequestDto);
        return CreatedAtAction(nameof(Show), new { categoryId, attributeId = attributeDto.Id }, attributeDto);
    }

    [HttpPatch("{attributeId:guid}")]
    public async Task<ActionResult<AttributeDto>> Update(
        Guid categoryId,
        Guid attributeId,
        UpdateAttributeRequestDto attributeRequestDto
    )
    {
        var attributeDto = await _attributeService.UpdateAttributeAsync(categoryId, attributeId, attributeRequestDto);
        return Ok(attributeDto);
    }

    [HttpDelete("{attributeId:guid}")]
    public async Task<IActionResult> Destroy(Guid categoryId, Guid attributeId)
    {
        await _attributeService.DeleteAttributeAsync(categoryId, attributeId);
        return NoContent();
    }
}