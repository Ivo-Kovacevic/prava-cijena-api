using api.Dto.Option;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;

namespace api.Services;

public class OptionService : IOptionService
{
    private readonly IAttributeRepository _attributeRepo;
    private readonly IOptionRepository _optionRepo;

    public OptionService(IAttributeRepository attributeRepository, IOptionRepository optionRepository)
    {
        _attributeRepo = attributeRepository;
        _optionRepo = optionRepository;
    }

    public async Task<IEnumerable<OptionDto>> GetOptionsByAttributeIdAsync(Guid attributeId)
    {
        var attributeExists = await _attributeRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var options = await _optionRepo.GetOptionsByAttributeIdAsync(attributeId);

        return options.Select(p => p.ToOptionDto());
    }

    public async Task<OptionDto> GetOptionByIdAsync(Guid attributeId, Guid optionId)
    {
        var attributeExists = await _attributeRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var option = await _optionRepo.GetOptionByIdAsync(optionId);
        if (option == null)
        {
            throw new NotFoundException($"Option with id '{optionId}' not found.");
        }

        return option.ToOptionDto();
    }

    public async Task<OptionDto> CreateOptionAsync(Guid attributeId, CreateOptionRequestDto optionRequestDto)
    {
        var attributeExists = await _attributeRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var option = optionRequestDto.OptionFromCreateRequestDto(attributeId);
        option = await _optionRepo.CreateAsync(option);

        return option.ToOptionDto();
    }

    public async Task<OptionDto> UpdateOptionAsync(
        Guid attributeId,
        Guid optionId,
        UpdateOptionRequestDto optionRequestDto
    )
    {
        var attributeExists = await _attributeRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var existingOption = await _optionRepo.GetOptionByIdAsync(optionId);
        if (existingOption == null)
        {
            throw new NotFoundException($"Option with id '{optionId}' not found.");
        }

        existingOption.ToOptionFromUpdateDto(optionRequestDto, attributeId);
        existingOption = await _optionRepo.UpdateAsync(existingOption);

        return existingOption.ToOptionDto();
    }

    public async Task DeleteOptionAsync(Guid attributeId, Guid optionId)
    {
        var attributeExists = await _attributeRepo.AttributeExists(attributeId);
        if (!attributeExists)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        var existingOption = await _optionRepo.GetOptionByIdAsync(optionId);
        if (existingOption == null)
        {
            throw new NotFoundException($"Option with id '{optionId}' not found.");
        }

        await _optionRepo.DeleteAsync(existingOption);
    }
}