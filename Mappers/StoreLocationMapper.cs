using PravaCijena.Api.Dto.StoreLocation;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Mappers;

public static class StoreLocationMapper
{
    public static StoreLocationDto ToStoreLocationDto(this StoreLocation storeLocation)
    {
        return new StoreLocationDto
        {
            Id = storeLocation.Id,
            City = storeLocation.City,
            Address = storeLocation.Address,
            StoreId = storeLocation.StoreId,
            LocationProduct = storeLocation.LocationProducts.Select(ps => ps.ToProductStoreDto()).FirstOrDefault(),
            CreatedAt = storeLocation.CreatedAt,
            UpdatedAt = storeLocation.UpdatedAt
        };
    }
}