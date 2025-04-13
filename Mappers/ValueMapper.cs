using PravaCijena.Api.Dto.Value;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Mappers;

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
            LabelId = attributeId
        };
    }

    public static ValueDto ToValueDto(this Value value)
    {
        return new ValueDto
        {
            Id = value.Id,
            Name = value.Name,
            Slug = value.Slug,
            LabelId = value.LabelId,
            CreatedAt = value.CreatedAt,
            UpdatedAt = value.UpdatedAt
        };
    }

    public static void ToValueFromUpdateDto(
        this Value existingValue,
        UpdateValueRequestDto valueRequestDto
    )
    {
        existingValue.Name = valueRequestDto.Name ?? existingValue.Name;
        existingValue.LabelId = valueRequestDto.LabelId ?? existingValue.LabelId;
    }
}