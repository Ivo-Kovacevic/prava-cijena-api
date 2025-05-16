using PravaCijena.Api.Dto.StoreLocation;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Store;

public class StoreWithMetadataDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public string? PriceListUrl { get; set; }
    public string? PriceUrlListXPath { get; set; }
    public string? PriceUrlXPath { get; set; }
    public string? PriceUrlType { get; set; }
    public int? BarcodeColumn { get; set; }
    public int? PriceColumn { get; set; }

    public string? DataLocation { get; set; }
    public string? BaseCategoryUrl { get; set; }
    public string? ProductListXPath { get; set; }
    public string? CatalogueListUrl { get; set; }
    public string? CatalogueListXPath { get; set; }
    public string? PageQuery { get; set; }
    public string? LimitQuery { get; set; }
    public required string StoreUrl { get; set; }
    public required string ImageUrl { get; set; }
    public List<StoreCategoryDto> Categories { get; set; } = [];
    public List<StoreLocationDto> StoreLocations { get; set; } = [];
}