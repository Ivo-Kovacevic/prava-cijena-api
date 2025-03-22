using api.Dto.Label;

namespace api.Interfaces;

public interface ILabelService
{
    Task<IEnumerable<LabelDto>> GetLabelsByCategoryIdAsync(Guid categoryId);
    Task<LabelDto> GetLabelByIdAsync(Guid categoryId, Guid attributeId);
    Task<LabelDto> CreateLabelAsync(Guid categoryId, CreateLabelRequestDto labelRequestDto);
    Task<LabelDto> UpdateLabelAsync(Guid categoryId, Guid attributeId, UpdateLabelRequestDto labelRequestDto);
    Task DeleteLabelAsync(Guid categoryId, Guid attributeId);
}