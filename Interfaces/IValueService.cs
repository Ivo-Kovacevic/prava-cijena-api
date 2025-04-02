using api.Dto.Value;

namespace api.Interfaces;

public interface IValueService
{
    Task<IEnumerable<ValueDto>> GetValuesByLabelIdAsync(Guid attributeId);
    Task<ValueDto> GetValueByIdAsync(Guid attributeId, Guid valueId);
    Task<ValueDto> CreateValueAsync(Guid attributeId, CreateValueRequestDto attributeRequestDto);
    Task<ValueDto> UpdateValueAsync(Guid attributeId, Guid valueId, UpdateValueRequestDto attributeRequestDto);
    Task DeleteValueAsync(Guid attributeId, Guid valueId);
}