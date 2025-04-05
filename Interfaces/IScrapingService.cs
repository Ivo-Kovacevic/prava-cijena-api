namespace PravaCijena.Api.Interfaces;

public interface IScrapingService
{
    Task<int> RunScraper();
}