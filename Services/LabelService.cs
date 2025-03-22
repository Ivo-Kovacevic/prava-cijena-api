using api.Dto.Label;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;

namespace api.Services;

public class LabelService : ILabelService
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly ILabelRepository _labelRepo;

    public LabelService(ICategoryRepository categoryRepository, ILabelRepository labelRepository)
    {
        _categoryRepo = categoryRepository;
        _labelRepo = labelRepository;
    }

    public async Task<IEnumerable<LabelDto>> GetLabelsByCategoryIdAsync(Guid categoryId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var labels = await _labelRepo.GetLabelsByCategoryIdAsync(categoryId);

        return labels.Select(p => p.ToLabelDto());
    }

    public async Task<LabelDto> GetLabelByIdAsync(Guid categoryId, Guid labelId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var label = await _labelRepo.GetLabelByIdAsync(labelId);
        if (label == null)
        {
            throw new NotFoundException($"Label with id '{labelId}' not found.");
        }

        return label.ToLabelDto();
    }

    public async Task<LabelDto> CreateLabelAsync(Guid categoryId, CreateLabelRequestDto labelRequestDto)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var product = labelRequestDto.LabelFromCreateRequestDto(categoryId);
        product = await _labelRepo.CreateAsync(product);

        return product.ToLabelDto();
    }

    public async Task<LabelDto> UpdateLabelAsync(
        Guid categoryId,
        Guid labelId,
        UpdateLabelRequestDto labelRequestDto
    )
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var existingLabel = await _labelRepo.GetLabelByIdAsync(labelId);
        if (existingLabel == null)
        {
            throw new NotFoundException($"Label with id '{labelId}' not found.");
        }

        existingLabel.ToLabelFromUpdateDto(labelRequestDto);
        existingLabel = await _labelRepo.UpdateAsync(existingLabel);

        return existingLabel.ToLabelDto();
    }

    public async Task DeleteLabelAsync(Guid categoryId, Guid labelId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var existingLabel = await _labelRepo.GetLabelByIdAsync(labelId);
        if (existingLabel == null)
        {
            throw new NotFoundException($"Label with id '{labelId}' not found.");
        }

        await _labelRepo.DeleteAsync(existingLabel);
    }
}