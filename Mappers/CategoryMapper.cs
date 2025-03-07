using api.Dto.Category;
using api.Helpers;
using api.Models;

namespace api.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToCategoryDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            ImageUrl = category.ImageUrl,
            ParentCategoryId = category.ParentCategoryId,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    public static Category CategoryFromCreateRequestDto(this CreateCategoryRequestDto categoryDto)
    {
        var name = categoryDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Category
        {
            Name = name,
            Slug = slug,
            ImageUrl = categoryDto.ImageUrl,
            ParentCategoryId = categoryDto.ParentCategoryId
        };
    }

    public static Category CategoryFromUpdateRequestDto(this UpdateCategoryRequestDto categoryRequestDto, Guid categoryId)
    {
        var name = categoryRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Category
        {
            Name = name,
            Slug = slug,
            ImageUrl = categoryRequestDto.ImageUrl,
            ParentCategoryId = categoryRequestDto.ParentCategoryId ?? categoryId
        };
    }
}