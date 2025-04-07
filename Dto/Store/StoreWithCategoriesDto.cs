using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Store;

public class StoreWithCategoriesDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string BaseUrl { get; set; }
    public required string ProductListXPath { get; set; }
    public required string StoreUrl { get; set; }
    public required string ImageUrl { get; set; }
    public List<StoreCategoryDto> Categories { get; set; } = [];
}