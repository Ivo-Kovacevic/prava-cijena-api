using api.Models;

namespace api.Interfaces;

public interface ILabelRepository
{
    Task<IEnumerable<Label>> GetLabelsByCategoryIdAsync(Guid categoryId);
    Task<Label?> GetLabelByIdAsync(Guid productId);
    Task<Label> CreateAsync(Label product);
    Task<Label> UpdateAsync(Label existingLabel);
    Task DeleteAsync(Label existingProduct);
    Task<bool> AttributeExists(Guid attributeId);
}