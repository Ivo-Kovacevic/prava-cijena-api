using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Seeders;

public class StoreCategorySeedingData : StoreSeedingData
{
    // Hardcoded GUIDs for StoreCategories
    public static readonly Guid KonzumDairyEggsId = new("7946ed46-1f9e-4fd4-a563-1590d9010222");
    public static readonly Guid KonzumDrinksId = new("b5fe073d-4f4a-4a42-85c7-6d815d8b0319");
    public static readonly Guid KonzumNonAlcoholicId = new("8f270ad4-b1bb-4184-a5cb-d3364fd01861");
    public static readonly Guid KonzumMilkId = new("3042487d-5f76-488d-9e08-d739fb82fffb");
    public static readonly Guid KonzumCheese = new("ff7ac1cb-a7b3-4216-9093-e2fde5ab5e09");
    public static readonly Guid KonzumEggsId = new("d842fa91-7f5c-4fc5-a94b-b7e9f57219c5");
    public static readonly Guid KonzumYogurtId = new("78a3dbcf-e535-4fe2-8c38-c5f85ad5299e");
    public static readonly Guid KonzumDessertsId = new("ae6315d3-9482-44a9-b7d0-73046242f3b9");
    public static readonly Guid KonzumButterFatId = new("8d465bb9-960f-4f56-9020-28f27c58d7fd");
    public static readonly Guid KonzumSpreadsId = new("bfa6829c-6b43-4ea7-9245-28db577a5155");

    public static readonly Guid KonzumBeerId = new("09b7d1de-85f3-4b92-b4f6-85b72e0e16d1");
    public static readonly Guid KonzumWineId = new("4e39d453-7867-49ef-81f4-b6f9d6a5e9c3");
    public static readonly Guid KonzumAlcoholId = new("f037daaf-f69f-41bb-8783-d20974db5667");

    public static readonly Guid KonzumWaterId = new("358b1d57-97ff-42db-905f-f937c27e1b53");
    public static readonly Guid KonzumCarbonatedId = new("433d8a24-b9e4-43a2-a6ae-5db750399b29");
    public static readonly Guid KonzumStillId = new("6f3ea824-5e3b-47a0-9fd7-464d8ad2f5a1");
    public static readonly Guid KonzumEnergyId = new("1ab55dbe-7a2c-437f-bba5-206231746e99");

    public static IEnumerable<StoreCategory> InitialStoreCategories()
    {
        return new List<StoreCategory>
        {
            // Top-level categories
            new() { Id = KonzumDairyEggsId, Name = "mlijecni-proizvodi-i-jaja", StoreId = KonzumStoreId },
            new() { Id = KonzumDrinksId, Name = "pica", StoreId = KonzumStoreId },

            // Dairy subcategories
            new()
            {
                Id = KonzumMilkId, Name = "mlijeko", ParentCategoryId = KonzumDairyEggsId
            },
            new() { Id = KonzumCheese, Name = "sirevi", ParentCategoryId = KonzumDairyEggsId },
            new() { Id = KonzumEggsId, Name = "jaja", ParentCategoryId = KonzumDairyEggsId },
            new() { Id = KonzumYogurtId, Name = "jogurti-i-ostalo", ParentCategoryId = KonzumDairyEggsId },
            new() { Id = KonzumDessertsId, Name = "mlijecni-deserti", ParentCategoryId = KonzumDairyEggsId },
            new() { Id = KonzumButterFatId, Name = "margarin-maslac-mast", ParentCategoryId = KonzumDairyEggsId },
            new() { Id = KonzumSpreadsId, Name = "namazi", ParentCategoryId = KonzumDairyEggsId },

            // Drinks subcategories
            new() { Id = KonzumNonAlcoholicId, Name = "bezalkoholna", ParentCategoryId = KonzumDrinksId },
            new() { Id = KonzumBeerId, Name = "pivo", ParentCategoryId = KonzumDrinksId },
            new() { Id = KonzumWineId, Name = "vino", ParentCategoryId = KonzumDrinksId },
            new() { Id = KonzumAlcoholId, Name = "alkoholna-pica", ParentCategoryId = KonzumDrinksId },

            // Sub-subcategories under "bezalkoholna"
            new() { Id = KonzumWaterId, Name = "voda", ParentCategoryId = KonzumNonAlcoholicId },
            new() { Id = KonzumCarbonatedId, Name = "gazirana", ParentCategoryId = KonzumNonAlcoholicId },
            new() { Id = KonzumStillId, Name = "negazirana", ParentCategoryId = KonzumNonAlcoholicId },
            new() { Id = KonzumEnergyId, Name = "energetska-pica", ParentCategoryId = KonzumNonAlcoholicId }
        };
    }
}