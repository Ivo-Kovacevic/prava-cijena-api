using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IAutomationService
{
    Task<AutomationResult> HandleFoundProducts(
        List<ProductPreview> productPreviews,
        StoreWithCategoriesDto store,
        Guid? equivalentCategoryId,
        AutomationResult results
    );
}