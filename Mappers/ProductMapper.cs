using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Mappers;

public static class ProductMapper
{
    public static ProductDto ToProductDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            LowestPrice = product.LowestPrice,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
        };
    }

    public static PageProductDto ToPageProductDto(this Product product)
    {
        return new PageProductDto
        {
            Id = product.Id,
            Name = product.Name,
            LowestPrice = product.LowestPrice,
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
            Barcode = productRequestDto.Barcode,
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

    public static void ToProductFromProductPreviewDto(
        this Product existingProduct,
        ProductPreview productPreview
    )
    {
        existingProduct.LowestPrice = productPreview.Price;
    }
}