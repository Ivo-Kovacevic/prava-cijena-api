using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Mappers;

public static class StoreMapper
{
    public static StoreDto ToStoreDto(this Store store)
    {
        return new StoreDto
        {
            Id = store.Id,
            Name = store.Name,
            StoreUrl = store.StoreUrl,
            ImageUrl = store.ImageUrl,
            BaseUrl = store.BaseCategoryUrl,
            CreatedAt = store.CreatedAt,
            UpdatedAt = store.UpdatedAt
        };
    }

    public static Store StoreFromCreateRequestDto(this CreateStoreRequestDto storeRequestDto)
    {
        var name = storeRequestDto.Name.Trim();
        var slug = SlugHelper.GenerateSlug(name);

        return new Store
        {
            Name = name,
            StoreUrl = storeRequestDto.StoreUrl,
            ImageUrl = storeRequestDto.ImageUrl
        };
    }

    public static void StoreFromUpdateRequestDto(this Store existingStore, UpdateStoreRequestDto storeRequestDto)
    {
        existingStore.Name = storeRequestDto.Name ?? existingStore.Name;
        existingStore.ImageUrl = storeRequestDto.ImageUrl ?? existingStore.ImageUrl;
    }
}