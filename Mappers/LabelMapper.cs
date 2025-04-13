using PravaCijena.Api.Dto.Label;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Mappers;

public static class LabelMapper
{
    public static Label LabelFromCreateRequestDto(this CreateLabelRequestDto labelRequestDto,
        Guid categoryId)
    {
        var name = labelRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Label
        {
            Name = name,
            CategoryId = categoryId
        };
    }

    public static LabelDto ToLabelDto(this Label label)
    {
        return new LabelDto
        {
            Id = label.Id,
            Name = label.Name,
            Slug = label.Slug,
            CategoryId = label.CategoryId,
            Values = label.Values.Select(v => v.ToValueDto()).ToList(),
            CreatedAt = label.CreatedAt,
            UpdatedAt = label.UpdatedAt
        };
    }

    public static void ToLabelFromUpdateDto(
        this Label existingLabel,
        UpdateLabelRequestDto labelRequestDto
    )
    {
        existingLabel.Name = labelRequestDto.Name ?? existingLabel.Name;
        existingLabel.CategoryId = labelRequestDto.CategoryId ?? existingLabel.CategoryId;
    }
}