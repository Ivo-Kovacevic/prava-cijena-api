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

    public static Category CategoryFromCreateRequestDto(this CreateCategoryRequestDto categoryRequestDto)
    {
        var name = categoryRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Category
        {
            Name = name,
            Slug = slug,
            ImageUrl = categoryRequestDto.ImageUrl,
            ParentCategoryId = categoryRequestDto.ParentCategoryId
        };
    }

    public static void CategoryFromUpdateRequestDto(
        this Category existingCategory,
        UpdateCategoryRequestDto categoryRequestDto
    )
    {
        existingCategory.Name = categoryRequestDto.Name ?? existingCategory.Name;
        existingCategory.Slug = SlugHelper.GenerateSlug(existingCategory.Name);
        existingCategory.ImageUrl = categoryRequestDto.ImageUrl ?? categoryRequestDto.ImageUrl;
        existingCategory.ParentCategoryId = categoryRequestDto.ParentCategoryId ?? categoryRequestDto.ParentCategoryId;
    }
}