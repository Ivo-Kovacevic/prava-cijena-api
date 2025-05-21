using System.Collections.Concurrent;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Context;

public class ProductProcessingContext
{
    public ConcurrentDictionary<string, byte> ExistingSlugs { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public ConcurrentDictionary<string, byte> ExistingBarcodes { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public async Task InitializeAsync(IProductRepository productRepository)
    {
        var slugs = await productRepository.GetAllSlugsAsync();
        ExistingSlugs = new ConcurrentDictionary<string, byte>(
            slugs.Select(slug => new KeyValuePair<string, byte>(slug, 0)),
            StringComparer.OrdinalIgnoreCase
        );

        var barcodes = await productRepository.GetAllBarcodesAsync();
        ExistingBarcodes = new ConcurrentDictionary<string, byte>(
            barcodes.Select(barcode => new KeyValuePair<string, byte>(barcode, 0)),
            StringComparer.OrdinalIgnoreCase
        );
    }
}