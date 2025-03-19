using api.Dto.Attribute;
using api.Helpers;
using Attribute = api.Models.Attribute;

namespace api.Mappers;

public static class AttributeMapper
{
    public static Attribute AttributeFromCreateRequestDto(this CreateAttributeRequestDto attributeRequestDto,
        Guid categoryId)
    {
        var name = attributeRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Attribute
        {
            Name = name,
            Slug = slug,
            CategoryId = categoryId
        };
    }

    public static AttributeDto ToAttributeDto(this Attribute attribute)
    {
        return new AttributeDto
        {
            Name = attribute.Name,
            Slug = attribute.Slug,
            CategoryId = attribute.CategoryId
        };
    }

    public static void ToAttributeFromUpdateDto(
        this Attribute existingAttribute,
        UpdateAttributeRequestDto attributeRequestDto,
        Guid categoryId
    )
    {
        existingAttribute.Name = attributeRequestDto.Name.Trim();
        existingAttribute.Slug = SlugHelper.GenerateSlug(existingAttribute.Name);
        existingAttribute.CategoryId = attributeRequestDto.CategoryId ?? categoryId;
    }
}