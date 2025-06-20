namespace PravaCijena.Api.Interfaces;

public interface IScrapingService
{
    Task RunScraper();
    Task AssignImages();
}