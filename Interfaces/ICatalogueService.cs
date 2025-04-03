namespace PravaCijena.Api.Interfaces;

public interface ICatalogueService
{
    Task<string> ExtractDataFromPdf(IFormFile pdfFile);
}