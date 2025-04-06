namespace PravaCijena.Api.Models.AutomationModels;

public class StoreScrapingConfig
{
    public string StoreSlug { get; set; }
    public string Url { get; set; }
    public string ProductListXPath { get; set; }
    public List<Category> Categories { get; set; }
}