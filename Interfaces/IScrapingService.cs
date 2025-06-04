namespace PravaCijena.Api.Interfaces;

public interface IScrapingService
{
    Task RunImageScraper();
    Task RunLinkScraper();
}