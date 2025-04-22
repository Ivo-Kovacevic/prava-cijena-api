using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface ICatalogueService
{
    Task<List<ProductPreview>> AnalyzePdf(IFormFile pdfFile);
}