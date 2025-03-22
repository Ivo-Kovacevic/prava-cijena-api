using api.Dto.Value;
using api.Helpers;
using api.Models;

namespace api.Mappers;

public static class ValueMapper
{
    public static Value ValueFromCreateRequestDto(this CreateValueRequestDto valueRequestDto,
        Guid attributeId)
    {
        var name = valueRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Value
        {
            Name = name,
            Slug = slug,
            AttributeId = attributeId
        };
    }

    public static ValueDto ToValueDto(this Value value)
    {
        return new ValueDto
        {
            Name = value.Name,
            Slug = value.Slug,
            AttributeId = value.AttributeId
        };
    }

    public static void ToValueFromUpdateDto(
        this Value existingValue,
        UpdateValueRequestDto valueRequestDto
    )
    {
        existingValue.Name = valueRequestDto.Name ?? existingValue.Name;
        existingValue.Slug = SlugHelper.GenerateSlug(existingValue.Name);
        existingValue.AttributeId = valueRequestDto.AttributeId ?? existingValue.AttributeId;
    }
}