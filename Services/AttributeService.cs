using api.Dto.Attribute;
using api.Exceptions;
using api.Interfaces;
using api.Mappers;

namespace api.Services;

public class AttributeService : IAttributeService
{
    private readonly IAttributeRepository _attributeRepo;
    private readonly ICategoryRepository _categoryRepo;

    public AttributeService(ICategoryRepository categoryRepository, IAttributeRepository attributeRepository)
    {
        _categoryRepo = categoryRepository;
        _attributeRepo = attributeRepository;
    }

    public async Task<IEnumerable<AttributeDto>> GetAttributesByCategoryIdAsync(Guid categoryId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var attributes = await _attributeRepo.GetAttributesByCategoryIdAsync(categoryId);

        return attributes.Select(p => p.ToAttributeDto());
    }

    public async Task<AttributeDto> GetAttributeByIdAsync(Guid categoryId, Guid attributeId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var attribute = await _attributeRepo.GetAttributeByIdAsync(attributeId);
        if (attribute == null)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        return attribute.ToAttributeDto();
    }

    public async Task<AttributeDto> CreateAttributeAsync(Guid categoryId, CreateAttributeRequestDto attributeRequestDto)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var product = attributeRequestDto.AttributeFromCreateRequestDto(categoryId);
        product = await _attributeRepo.CreateAsync(product);

        return product.ToAttributeDto();
    }

    public async Task<AttributeDto> UpdateAttributeAsync(
        Guid categoryId,
        Guid attributeId,
        UpdateAttributeRequestDto attributeRequestDto
    )
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var existingAttribute = await _attributeRepo.GetAttributeByIdAsync(attributeId);
        if (existingAttribute == null)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        existingAttribute.ToAttributeFromUpdateDto(attributeRequestDto, categoryId);
        existingAttribute = await _attributeRepo.UpdateAsync(existingAttribute);

        return existingAttribute.ToAttributeDto();
    }

    public async Task DeleteAttributeAsync(Guid categoryId, Guid attributeId)
    {
        var categoryExists = await _categoryRepo.CategoryExists(categoryId);
        if (!categoryExists)
        {
            throw new NotFoundException($"Category with id '{categoryId}' not found.");
        }

        var existingAttribute = await _attributeRepo.GetAttributeByIdAsync(attributeId);
        if (existingAttribute == null)
        {
            throw new NotFoundException($"Attribute with id '{attributeId}' not found.");
        }

        await _attributeRepo.DeleteAsync(existingAttribute);
    }
}