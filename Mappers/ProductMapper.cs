using api.DTOs;
using api.Helpers;
using api.Models;

namespace api.Mappers;

public static class ProductMapper
{
    public static ProductDto ToProductDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
    
    public static Product ProductFromCreateRequestDto(this CreateProductRequestDto productRequestDto, Guid categoryId)
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
    
    public static Product ToProductFromUpdateDto(this UpdateProductRequestDto productRequestDto, Guid categoryId)
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