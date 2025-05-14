using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Seeders;

public class StoreLocationSeedingData : StoreSeedingData
{
    public static IEnumerable<StoreLocation> InitialStoreLocations()
    {
        return new List<StoreLocation>
        {
            new()
            {
                Id = Guid.Parse("a3c9e9c9-41c5-4f11-a3f6-0ff87b6711f1"),
                City = "Split",
                Address = "114. brigade 6",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("b79ea2bb-0e1b-421e-9f0d-b9b0c679e9fc"),
                City = "Zadar",
                Address = "Andrije Hebranga 2",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("5396ff99-69e6-4206-9621-15e0ceba6f67"),
                City = "Zagreb",
                Address = "Antuna Soljana 43",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("51fc62e0-bfe4-4a89-86db-89ce310474f4"),
                City = "Zagreb",
                Address = "Bistricka 6",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("cc96eb4b-79cd-4a89-9db0-37f90a6e354f"),
                City = "Zagreb",
                Address = "Jablanska ulica br. 80",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("72ae7784-bec8-4a2f-9027-93c30b43b13e"),
                City = "Zagreb",
                Address = "Jaruscica 6",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("cd81c8ce-5b8f-4f99-b88b-f6ae72809f49"),
                City = "Osijek",
                Address = "Josipa Reihl-Kira 40",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("89e6c5ff-c0b6-4cd2-9d3d-146d8619ac4d"),
                City = "Zagreb",
                Address = "Julija Knifera 1",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("9e640c64-4fd6-456c-8dc9-b97dce857b4e"),
                City = "Pula",
                Address = "Jurja Zakna 3",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("c5c3d402-b761-42b6-93dc-ea79e00ab14b"),
                City = "Zagreb",
                Address = "Kneza Branimira 119",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("26ad7e9d-55ef-45b4-91dc-3c7f70f6f300"),
                City = "Kutina",
                Address = "Kneza Lj. Posavskog 32",
                StoreId = KauflandStoreId
            },
            new()
            {
                Id = Guid.Parse("d1b6de66-1d57-4ec1-87ce-2b68931a148c"),
                City = "Trogir",
                Address = "Kneza Trpimira 301",
                StoreId = KauflandStoreId
            }
        };
    }
}