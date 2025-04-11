using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IScrapingService
{
    Task<AutomationResult> RunScraper();
}