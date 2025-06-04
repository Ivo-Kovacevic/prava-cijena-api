namespace PravaCijena.Api.Models;

public class Store : BaseNamedEntity
{
    public required string StoreUrl { get; set; }

    // Structured data info
    public string? PriceListUrl { get; set; }
    public string? PriceUrlListXPath { get; set; }
    public string? PriceUrlXPath { get; set; }
    public string? PriceUrlType { get; set; }
    public string? DataLocation { get; set; }
    public int? CsvNameColumn { get; set; }
    public int? CsvBrandColumn { get; set; }
    public int? CsvBarcodeColumn { get; set; }
    public int? CsvPriceColumn { get; set; }
    public string? CsvDelimiter { get; set; }
    public string? XmlNameElement { get; set; }
    public string? XmlBrandElement { get; set; }
    public string? XmlBarcodeElement { get; set; }
    public string? XmlPriceElement { get; set; }

    // Scraping info
    public string? BaseCategoryUrl { get; set; }
    public string? ProductListXPath { get; set; }

    // Catalogue analysing info
    public string? CatalogueListUrl { get; set; }
    public string? CatalogueListXPath { get; set; }

    public required string ImageUrl { get; set; }
    public string? PageQuery { get; set; }
    public string? LimitQuery { get; set; }
    public List<StoreCategory> Categories { get; set; }
    public List<StoreLocation> StoreLocations { get; set; }
    public List<ProductStoreLink> StoreProductLinks { get; set; }
}