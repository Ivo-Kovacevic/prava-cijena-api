using api.Dto.Option;

namespace api.Interfaces;

public interface IOptionService
{
    Task<IEnumerable<OptionDto>> GetOptionsByAttributeIdAsync(Guid attributeId);
    Task<OptionDto> GetOptionByIdAsync(Guid attributeId, Guid optionId);
    Task<OptionDto> CreateOptionAsync(Guid attributeId, CreateOptionRequestDto attributeRequestDto);
    Task<OptionDto> UpdateOptionAsync(Guid attributeId, Guid optionId, UpdateOptionRequestDto attributeRequestDto);
    Task DeleteOptionAsync(Guid attributeId, Guid optionId);
}