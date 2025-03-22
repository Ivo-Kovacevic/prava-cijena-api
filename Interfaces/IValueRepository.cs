using api.Models;

namespace api.Interfaces;

public interface IValueRepository
{
    Task<IEnumerable<Value>> GetValuesByAttributeIdAsync(Guid attributeId);
    Task<Value?> GetValueByIdAsync(Guid productId);
    Task<Value> CreateAsync(Value value);
    Task<Value> UpdateAsync(Value existingValue);
    Task DeleteAsync(Value existingProduct);
}