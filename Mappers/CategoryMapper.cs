using System.Text.RegularExpressions;
using api.DTOs.Category;
using api.Helpers;
using api.Models;

namespace api.Mappers;

public static class CategoryMapper
{
    public static Category CreateCategoryFromDto(this CreateCategoryRequestDto categoryDto)
    {
        var name = categoryDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Category
        {
            Name = name,
            Slug = slug,
            ImageUrl = categoryDto.ImageUrl
        };
    }
    
    public static Category UpdateCategoryFromDto(this UpdateCategoryRequestDto categoryDto)
    {
        var name = categoryDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Category
        {
            Name = name,
            Slug = slug,
            ImageUrl = categoryDto.ImageUrl
        };
    }
}