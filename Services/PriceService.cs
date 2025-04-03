using PravaCijena.Api.Dto.Price;
using PravaCijena.Api.Exceptions;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Mappers;

namespace PravaCijena.Api.Services;

public class PriceService : IPriceService
{
    private readonly IPriceRepository _priceRepo;
    private readonly IProductStoreRepository _productStoreRepo;

    public PriceService(IProductStoreRepository productStoreRepository, IPriceRepository priceRepository)
    {
        _productStoreRepo = productStoreRepository;
        _priceRepo = priceRepository;
    }

    public async Task<IEnumerable<PriceDto>> GetPricesAsync(Guid productStoreId)
    {
        var productStoreExists = await _productStoreRepo.ProductStoreExistsAsync(productStoreId);
        if (!productStoreExists)
        {
            throw new NotFoundException($"ProductStore with id '{productStoreId}' not found.");
        }

        var products = await _priceRepo.GetPricesByProductStoreIdAsync(productStoreId);

        return products.Select(p => p.ToPriceDto());
    }

    public async Task<PriceDto> GetPriceByIdAsync(Guid productStoreId, Guid priceId)
    {
        var productStoreExists = await _productStoreRepo.ProductStoreExistsAsync(productStoreId);
        if (!productStoreExists)
        {
            throw new NotFoundException($"ProductStore with id '{productStoreId}' not found.");
        }

        var price = await _priceRepo.GetPriceByIdAsync(priceId);
        if (price == null)
        {
            throw new NotFoundException($"Price with id '{priceId}' not found.");
        }

        return price.ToPriceDto();
    }

    public async Task<PriceDto> CreatePriceAsync(Guid productStoreId, CreatePriceRequestDto priceRequestDto)
    {
        var productStoreExists = await _productStoreRepo.ProductStoreExistsAsync(productStoreId);
        if (!productStoreExists)
        {
            throw new NotFoundException($"ProductStore with id '{productStoreId}' not found.");
        }

        var price = priceRequestDto.PriceFromCreateRequestDto(productStoreId);
        price = await _priceRepo.CreateAsync(price);

        return price.ToPriceDto();
    }

    public async Task<PriceDto> UpdatePriceAsync(
        Guid productStoreId,
        Guid priceId,
        UpdatePriceRequestDto priceRequestDto
    )
    {
        var productStoreExists = await _productStoreRepo.ProductStoreExistsAsync(productStoreId);
        if (!productStoreExists)
        {
            throw new NotFoundException($"ProductStore with id '{productStoreId}' not found.");
        }

        var existingPrice = await _priceRepo.GetPriceByIdAsync(priceId);
        if (existingPrice == null)
        {
            throw new NotFoundException($"Price with id '{priceId}' not found.");
        }

        existingPrice.ToPriceFromUpdateDto(priceRequestDto);
        existingPrice = await _priceRepo.UpdateAsync(existingPrice);

        return existingPrice.ToPriceDto();
    }

    public async Task DeletePriceAsync(Guid productStoreId, Guid priceId)
    {
        var productStoreExists = await _productStoreRepo.ProductStoreExistsAsync(productStoreId);
        if (!productStoreExists)
        {
            throw new NotFoundException($"ProductStore with id '{productStoreId}' not found.");
        }

        var existingPrice = await _priceRepo.GetPriceByIdAsync(priceId);
        if (existingPrice == null)
        {
            throw new NotFoundException($"Price with id '{priceId}' not found.");
        }

        await _priceRepo.DeleteAsync(existingPrice);
    }
}