using api.Dto.Label;
using api.Helpers;
using api.Models;

namespace api.Mappers;

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