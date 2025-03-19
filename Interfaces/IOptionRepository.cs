using api.Models;

namespace api.Interfaces;

public interface IOptionRepository
{
    Task<IEnumerable<Option>> GetOptionsByAttributeIdAsync(Guid categoryId);
    Task<Option?> GetOptionByIdAsync(Guid productId);
    Task<Option> CreateAsync(Option product);
    Task<Option> UpdateAsync(Option existingOption);
    Task DeleteAsync(Option existingProduct);
}