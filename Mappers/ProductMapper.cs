using api.DTOs;
using api.Helpers;
using api.Models;

namespace api.Mappers;

public static class ProductMapper
{
    public static Product CreateProductFromDto(this CreateProductRequestDto productRequestDto, int categoryId)
    {
        var name = productRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Product
        {
            Name = name,
            Slug = slug,
            ImageUrl = productRequestDto.ImageUrl,
            CategoryId = categoryId
        };
    }
    
    public static Product UpdateProductFromDto(this UpdateProductRequestDto productRequestDto, int categoryId)
    {
        var name = productRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Product
        {
            Name = name,
            Slug = slug,
            ImageUrl = productRequestDto.ImageUrl,
            CategoryId = categoryId
        };
    }
}