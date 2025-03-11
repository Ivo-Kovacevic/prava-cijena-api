using api.Dto.Attribute;

namespace api.Interfaces;

public interface IAttributeService
{
    Task<IEnumerable<AttributeDto>> GetAttributesByCategoryIdAsync(Guid categoryId);
    Task<AttributeDto> GetAttributeByIdAsync(Guid categoryId, Guid attributeId);
    Task<AttributeDto> CreateAttributeAsync(Guid categoryId, CreateAttributeRequestDto attributeRequestDto);
    Task<AttributeDto> UpdateAttributeAsync(Guid categoryId, Guid attributeId, UpdateAttributeRequestDto attributeRequestDto);
    Task DeleteAttributeAsync(Guid categoryId, Guid attributeId);
}