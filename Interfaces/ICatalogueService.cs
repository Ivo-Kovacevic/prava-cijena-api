namespace api.Interfaces;

public interface ICatalogueService
{
    Task<string> ExtractDataFromPdf(IFormFile pdfFile);
}