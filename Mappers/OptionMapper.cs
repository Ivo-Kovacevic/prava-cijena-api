using api.Dto.Option;
using api.Helpers;
using api.Models;

namespace api.Mappers;

public static class OptionMapper
{
    public static Option OptionFromCreateRequestDto(this CreateOptionRequestDto optionRequestDto,
        Guid attributeId)
    {
        var name = optionRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Option
        {
            Name = name,
            Slug = slug,
            AttributeId = attributeId
        };
    }

    public static OptionDto ToOptionDto(this Option option)
    {
        return new OptionDto
        {
            Name = option.Name,
            Slug = option.Slug,
            AttributeId = option.AttributeId
        };
    }

    public static void ToOptionFromUpdateDto(
        this Option existingOption,
        UpdateOptionRequestDto optionRequestDto
    )
    {
        existingOption.Name = optionRequestDto.Name ?? existingOption.Name;
        existingOption.Slug = SlugHelper.GenerateSlug(existingOption.Name);
        existingOption.AttributeId = optionRequestDto.AttributeId ?? existingOption.AttributeId;
    }
}