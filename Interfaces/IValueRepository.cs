using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IValueRepository
{
    Task<IEnumerable<Value>> GetValuesByLabelIdAsync(Guid attributeId);
    Task<Value?> GetValueByIdAsync(Guid productId);
    Task<Value> CreateAsync(Value value);
    Task<Value> UpdateAsync(Value existingValue);
    Task DeleteAsync(Value existingProduct);
}