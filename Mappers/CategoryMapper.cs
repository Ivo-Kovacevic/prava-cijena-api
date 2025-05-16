using PravaCijena.Api.Dto.Category;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Mappers;

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
            HexColor = category.HexColor,
            ParentCategoryId = category.ParentCategoryId,
            Subcategories = category.Subcategories.Select(c => c.ToCategoryDto()).ToList(),
            Labels = category.Labels.Select(l => l.ToLabelDto()).ToList(),
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    public static BaseCategory ToBaseCategory(this Category category)
    {
        return new BaseCategory
        {
            Id = category.Id,
            Name = category.Name,
            SubCategories = category.Subcategories.Select(c=>c.ToBaseCategory()).ToList()
        };
    }

    public static Category CategoryFromCreateRequestDto(this CreateCategoryRequestDto categoryRequestDto)
    {
        var name = categoryRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Category
        {
            Name = name,
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
        existingCategory.ImageUrl = categoryRequestDto.ImageUrl ?? categoryRequestDto.ImageUrl;
        existingCategory.ParentCategoryId = categoryRequestDto.ParentCategoryId ?? categoryRequestDto.ParentCategoryId;
    }
}