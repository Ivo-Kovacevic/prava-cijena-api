using PravaCijena.Api.Mappers;
using PravaCijena.Api.Dto.Value;
using PravaCijena.Api.Exceptions;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Services;

public class ValueService : IValueService
{
    private readonly ILabelRepository _labelRepo;
    private readonly IValueRepository _valueRepo;

    public ValueService(ILabelRepository labelRepository, IValueRepository valueRepository)
    {
        _labelRepo = labelRepository;
        _valueRepo = valueRepository;
    }

    public async Task<IEnumerable<ValueDto>> GetValuesByLabelIdAsync(Guid labelId)
    {
        var labelExists = await _labelRepo.LabelExists(labelId);
        if (!labelExists)
        {
            throw new NotFoundException($"Label with id '{labelId}' not found.");
        }

        var values = await _valueRepo.GetValuesByLabelIdAsync(labelId);

        return values.Select(p => p.ToValueDto());
    }

    public async Task<ValueDto> GetValueByIdAsync(Guid labelId, Guid valueId)
    {
        var labelExists = await _labelRepo.LabelExists(labelId);
        if (!labelExists)
        {
            throw new NotFoundException($"Label with id '{labelId}' not found.");
        }

        var value = await _valueRepo.GetValueByIdAsync(valueId);
        if (value == null)
        {
            throw new NotFoundException($"Value with id '{valueId}' not found.");
        }

        return value.ToValueDto();
    }

    public async Task<ValueDto> CreateValueAsync(Guid labelId, CreateValueRequestDto valueRequestDto)
    {
        var labelExists = await _labelRepo.LabelExists(labelId);
        if (!labelExists)
        {
            throw new NotFoundException($"Label with id '{labelId}' not found.");
        }

        var value = valueRequestDto.ValueFromCreateRequestDto(labelId);
        value = await _valueRepo.CreateAsync(value);

        return value.ToValueDto();
    }

    public async Task<ValueDto> UpdateValueAsync(
        Guid labelId,
        Guid valueId,
        UpdateValueRequestDto valueRequestDto
    )
    {
        var labelExists = await _labelRepo.LabelExists(labelId);
        if (!labelExists)
        {
            throw new NotFoundException($"Label with id '{labelId}' not found.");
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

    public async Task DeleteValueAsync(Guid labelId, Guid valueId)
    {
        var labelExists = await _labelRepo.LabelExists(labelId);
        if (!labelExists)
        {
            throw new NotFoundException($"Label with id '{labelId}' not found.");
        }

        var existingValue = await _valueRepo.GetValueByIdAsync(valueId);
        if (existingValue == null)
        {
            throw new NotFoundException($"Value with id '{valueId}' not found.");
        }

        await _valueRepo.DeleteAsync(existingValue);
    }
}