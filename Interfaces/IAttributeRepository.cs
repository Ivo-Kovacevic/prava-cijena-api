using Attribute = api.Models.Attribute;

namespace api.Interfaces;

using Attribute = Attribute;

public interface IAttributeRepository
{
    Task<IEnumerable<Attribute>> GetAttributesByCategoryIdAsync(Guid categoryId);
    Task<Attribute?> GetAttributeByIdAsync(Guid productId);
    Task<Attribute> CreateAsync(Attribute product);
    Task<Attribute> UpdateAsync(Attribute existingAttribute);
    Task DeleteAsync(Attribute existingProduct);
}