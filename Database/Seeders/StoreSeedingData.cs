using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Seeders;

public class StoreSeedingData
{
    public static readonly Guid KonzumStoreId = new("c25d8a61-eac0-4f5e-a1f5-d9f80b6b2d4e");
    public static readonly Guid LidlStoreId = new("137a4c4a-2d10-4037-a65d-dc1f86d77d2e");
    public static readonly Guid StudenacStoreId = new("63617c67-553b-4a06-920e-97c167d1d511");
    public static readonly Guid TommyStoreId = new("3fd0a5e6-d064-4dcb-a0ae-83a9021dd099");
    public static readonly Guid SparStoreId = new("d8a4e1bb-1a99-41de-b14e-b9c93b35f0df");
    public static readonly Guid EurospinStoreId = new("588ff019-f46a-4f2f-8eb2-3fd4a9bfbfb9");
    public static readonly Guid KauflandStoreId = new("5ce8d6a7-2d5e-4a69-9e4f-750b4e314134");
    public static readonly Guid PlodineStoreId = new("c52cf217-b75a-4f41-bb79-77fa2956df0b");
    public static readonly Guid MetroStoreId = new("1649011f-203f-4e89-8a6e-c1f9a7d6d2f3");
    public static readonly Guid NtlStoreId = new("77c19bc6-25a7-4e62-bd92-5b155cbf66d2");

    public static IEnumerable<Store> InitialStores()
    {
        return new List<Store>
        {
            new()
            {
                Id = KonzumStoreId,
                Name = "Konzum",
                StoreUrl = "https://www.konzum.hr",
                BaseUrl = "https://www.konzum.hr/web/t/kategorije",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/konzum.webp",
                ProductListXPath =
                    "//div[@class='product-list product-list--md-5 js-product-layout-container product-list--grid']//article",
                PageQuery = "page",
                LimitQuery = "per_page"
            },
            new()
            {
                Id = LidlStoreId,
                Name = "Lidl",
                StoreUrl = "https://www.lidl.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/lidl.webp"
            },
            new()
            {
                Id = StudenacStoreId,
                Name = "Studenac",
                StoreUrl = "https://www.studenac.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/studenac.webp"
            },
            new()
            {
                Id = TommyStoreId,
                Name = "Tommy",
                StoreUrl = "https://www.tommy.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/tommy.webp"
            },
            new()
            {
                Id = SparStoreId,
                Name = "Spar",
                StoreUrl = "https://www.spar.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/spar.webp"
            },
            new()
            {
                Id = EurospinStoreId,
                Name = "Eurospin",
                StoreUrl = "https://www.eurospin.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/eurospin.webp"
            },
            new()
            {
                Id = KauflandStoreId,
                Name = "Kaufland",
                StoreUrl = "https://www.kaufland.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/kaufland.webp"
            },
            new()
            {
                Id = PlodineStoreId,
                Name = "Plodine",
                StoreUrl = "https://www.plodine.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/plodine.webp"
            },
            new()
            {
                Id = MetroStoreId,
                Name = "Metro",
                StoreUrl = "https://www.metro-cc.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/metro.webp"
            },
            new()
            {
                Id = NtlStoreId,
                Name = "NTL",
                StoreUrl = "https://www.ntl.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/v1743770051/ntl.webp"
            }
        };
    }
}