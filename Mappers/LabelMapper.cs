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
            Slug = slug,
            CategoryId = categoryId
        };
    }

    public static LabelDto ToLabelDto(this Label label)
    {
        return new LabelDto
        {
            Name = label.Name,
            Slug = label.Slug,
            CategoryId = label.CategoryId
        };
    }

    public static void ToLabelFromUpdateDto(
        this Label existingLabel,
        UpdateLabelRequestDto labelRequestDto
    )
    {
        existingLabel.Name = labelRequestDto.Name ?? existingLabel.Name;
        existingLabel.Slug = SlugHelper.GenerateSlug(existingLabel.Name);
        existingLabel.CategoryId = labelRequestDto.CategoryId ?? existingLabel.CategoryId;
    }
}