using PravaCijena.Api.Dto.Label;

namespace PravaCijena.Api.Interfaces;

public interface ILabelService
{
    Task<IEnumerable<LabelDto>> GetLabelsByCategorySlugAsync(string categorySlug);
    Task<LabelDto> GetLabelBySlugAsync(string labelSlug);
    Task<IEnumerable<LabelDto>> GetLabelsByCategoryIdAsync(Guid categoryId);
    Task<LabelDto> GetLabelByIdAsync(Guid categoryId, Guid attributeId);
    Task<LabelDto> CreateLabelAsync(Guid categoryId, CreateLabelRequestDto labelRequestDto);
    Task<LabelDto> UpdateLabelAsync(Guid categoryId, Guid attributeId, UpdateLabelRequestDto labelRequestDto);
    Task DeleteLabelAsync(Guid categoryId, Guid attributeId);
}