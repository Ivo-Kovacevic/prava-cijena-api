using api.Dto.Store;
using api.Helpers;
using api.Models;

namespace api.Mappers;

public static class StoreMapper
{
    public static StoreDto ToStoreDto(this Store store)
    {
        return new StoreDto
        {
            Id = store.Id,
            Name = store.Name,
            Slug = store.Slug,
            StoreUrl = store.StoreUrl,
            ImageUrl = store.ImageUrl,
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
            Slug = slug,
            StoreUrl = storeRequestDto.StoreUrl,
            ImageUrl = storeRequestDto.ImageUrl,
        };
    }

    public static void StoreFromUpdateRequestDto(this Store existingStore, UpdateStoreRequestDto storeRequestDto)
    {
        existingStore.Name = storeRequestDto.Name.Trim();
        existingStore.Slug = SlugHelper.GenerateSlug(existingStore.Name);
        existingStore.ImageUrl = storeRequestDto.ImageUrl;
    }
}