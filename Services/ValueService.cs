using api.Dto.Value;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;

namespace api.Services;

public class ValueService : IValueService
{
    private readonly ILabelRepository _labelRepo;
    private readonly IValueRepository _valueRepo;

    public ValueService(ILabelRepository labelRepository, IValueRepository valueRepository)
    {
        _labelRepo = labelRepository;
        _valueRepo = valueRepository;
    }

    public async Task<IEnumerable<ValueDto>> GetValuesByAttributeIdAsync(Guid attributeId)
    {
        var attributeExists = await _labelRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var values = await _valueRepo.GetValuesByAttributeIdAsync(attributeId);

        return values.Select(p => p.ToValueDto());
    }

    public async Task<ValueDto> GetValueByIdAsync(Guid attributeId, Guid valueId)
    {
        var attributeExists = await _labelRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var value = await _valueRepo.GetValueByIdAsync(valueId);
        if (value == null)
        {
            throw new NotFoundException($"Value with id '{valueId}' not found.");
        }

        return value.ToValueDto();
    }

    public async Task<ValueDto> CreateValueAsync(Guid attributeId, CreateValueRequestDto valueRequestDto)
    {
        var attributeExists = await _labelRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var value = valueRequestDto.ValueFromCreateRequestDto(attributeId);
        value = await _valueRepo.CreateAsync(value);

        return value.ToValueDto();
    }

    public async Task<ValueDto> UpdateValueAsync(
        Guid attributeId,
        Guid valueId,
        UpdateValueRequestDto valueRequestDto
    )
    {
        var attributeExists = await _labelRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var existingValue = await _valueRepo.GetValueByIdAsync(valueId);
        if (existingValue == null)
        {
            throw new NotFoundException($"Value with id '{valueId}' not found.");
        }

        existingValue.ToValueFromUpdateDto(valueRequestDto);
        existingValue = await _valueRepo.UpdateAsync(existingValue);

        return existingValue.ToValueDto();
    }

    public async Task DeleteValueAsync(Guid attributeId, Guid valueId)
    {
        var attributeExists = await _labelRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var existingValue = await _valueRepo.GetValueByIdAsync(valueId);
        if (existingValue == null)
        {
            throw new NotFoundException($"Value with id '{valueId}' not found.");
        }

        await _valueRepo.DeleteAsync(existingValue);
    }
}