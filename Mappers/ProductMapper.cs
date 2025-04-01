using api.Dto.Product;
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
            ImageUrl = productRequestDto.ImageUrl,
            CategoryId = categoryId
        };
    }

    public static void ToProductFromUpdateDto(
        this Product existingProduct,
        UpdateProductRequestDto productRequestDto
    )
    {
        existingProduct.Name = productRequestDto.Name ?? existingProduct.Name;
        existingProduct.ImageUrl = productRequestDto.ImageUrl ?? existingProduct.ImageUrl;
        existingProduct.CategoryId = productRequestDto.CategoryId ?? existingProduct.CategoryId;
    }
}