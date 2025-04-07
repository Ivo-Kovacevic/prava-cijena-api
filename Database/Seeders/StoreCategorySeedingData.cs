using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Seeders;

public class StoreCategorySeedingData
{
    // Konzum categories
    public static readonly Guid KonzumDairyEggsId = new("7946ed46-1f9e-4fd4-a563-1590d9010222");
    public static readonly Guid KonzumMilkId = new("3042487d-5f76-488d-9e08-d739fb82fffb");
    public static readonly Guid KonzumCheese = new("ff7ac1cb-a7b3-4216-9093-e2fde5ab5e09");
    public static readonly Guid KonzumEggsId = new("d842fa91-7f5c-4fc5-a94b-b7e9f57219c5");
    public static readonly Guid KonzumYogurtId = new("78a3dbcf-e535-4fe2-8c38-c5f85ad5299e");
    public static readonly Guid KonzumDessertsId = new("ae6315d3-9482-44a9-b7d0-73046242f3b9");
    public static readonly Guid KonzumButterFatId = new("8d465bb9-960f-4f56-9020-28f27c58d7fd");
    public static readonly Guid KonzumSpreadsId = new("bfa6829c-6b43-4ea7-9245-28db577a5155");

    public static readonly Guid KonzumDrinksId = new("b5fe073d-4f4a-4a42-85c7-6d815d8b0319");
    public static readonly Guid KonzumNonAlcoholicId = new("8f270ad4-b1bb-4184-a5cb-d3364fd01861");
    public static readonly Guid KonzumBeerId = new("09b7d1de-85f3-4b92-b4f6-85b72e0e16d1");
    public static readonly Guid KonzumWineId = new("4e39d453-7867-49ef-81f4-b6f9d6a5e9c3");
    public static readonly Guid KonzumAlcoholId = new("f037daaf-f69f-41bb-8783-d20974db5667");
    public static readonly Guid KonzumWaterId = new("358b1d57-97ff-42db-905f-f937c27e1b53");
    public static readonly Guid KonzumCarbonatedId = new("433d8a24-b9e4-43a2-a6ae-5db750399b29");
    public static readonly Guid KonzumStillId = new("6f3ea824-5e3b-47a0-9fd7-464d8ad2f5a1");
    public static readonly Guid KonzumEnergyId = new("1ab55dbe-7a2c-437f-bba5-206231746e99");

    // Tommy categories
    public static readonly Guid TommyDairyEggsId = new("194aad3f-d0e7-4ace-9781-3b534b63bd76");
    public static readonly Guid TommyEggsId = new("8219aa22-6af7-431b-b8eb-70b2c53c459c");
    public static readonly Guid TommyButterFatId = new("fe378902-103a-43fd-a5c5-77b9179c1cf2");
    public static readonly Guid TommyCheese = new("860a9f63-5988-425a-b2cd-df521892299a");
    public static readonly Guid TommyDessertsId = new("83c64ddb-7d41-43cc-b92f-09d7e769ba59");
    public static readonly Guid TommyCreamsId = new("dfa3993e-9839-4acf-8098-766c5bbbc38a");
    public static readonly Guid TommySpreadsId = new("cc8d02b4-cfb6-4fd6-8f97-f11637bfc086");
    public static readonly Guid TommyMilkId = new("cda10bb7-29a2-4d79-9846-afbc8002c96e");
    public static readonly Guid TommyYogurtId = new("932e9afa-0d1c-45c4-ae91-fbc2affca2a6");
    public static readonly Guid TommyIceCoffeeId = new("cbe54ef8-2dae-4407-be6a-58e75f6094d8");
    public static readonly Guid TommyDairyDrinksId = new("28167989-33bc-4874-b219-279400740e36");

    public static readonly Guid TommyDrinksId = new("feefc22f-0cdd-49d6-b014-0033d2fe4357");
    public static readonly Guid TommyNonAlcoholicId = new("64087996-e5b0-4081-a13c-82f379916d6d");
    public static readonly Guid TommyBeerId = new("a0e7de4d-51f2-459b-9151-9640eb0dbd65");
    public static readonly Guid TommyWineId = new("7030e7f9-ec2a-4482-a0c0-0fc47d7cba46");
    public static readonly Guid TommyStrongAlcoholId = new("a93d98de-2fb0-4f3a-854b-b9b101ee62e6");
    public static readonly Guid TommyCarbonatedId = new("9cf6adf9-fbac-45ec-9530-ec016904347b");
    public static readonly Guid TommyWaterId = new("e53d0fc7-4586-46c1-aa5e-a5fa4e83d8a2");
    public static readonly Guid TommyStillId = new("97518b45-fb79-4945-939f-6829c6eda29e");
    public static readonly Guid TommyEnergyId = new("8517af13-fef6-49eb-a0b2-55632e0b0480");

    public static IEnumerable<StoreCategory> InitialStoreCategories()
    {
        return new List<StoreCategory>
        {
            /*
             * Konzum categories
             */
            // Top-level categories
            new()
            {
                Id = KonzumDairyEggsId, Name = "mlijecni-proizvodi-i-jaja", StoreId = StoreSeedingData.KonzumStoreId
            },
            new() { Id = KonzumDrinksId, Name = "pica", StoreId = StoreSeedingData.KonzumStoreId },

            // Dairy subcategories
            new()
            {
                Id = KonzumMilkId, Name = "mlijeko",
                ParentCategoryId = KonzumDairyEggsId,
                EquivalentCategoryId = CategorySeedingData.MilkSubcategoryId
            },
            new()
            {
                Id = KonzumCheese, Name = "sirevi",
                ParentCategoryId = KonzumDairyEggsId,
                EquivalentCategoryId = CategorySeedingData.CheeseSubcategoryId
            },
            new()
            {
                Id = KonzumEggsId, Name = "jaja",
                ParentCategoryId = KonzumDairyEggsId,
                EquivalentCategoryId = CategorySeedingData.EggsSubcategoryId
            },
            new()
            {
                Id = KonzumYogurtId, Name = "jogurti-i-ostalo",
                ParentCategoryId = KonzumDairyEggsId,
                EquivalentCategoryId = CategorySeedingData.YogurtSubcategoryId
            },
            new() { Id = KonzumDessertsId, Name = "mlijecni-deserti", ParentCategoryId = KonzumDairyEggsId },
            new()
            {
                Id = KonzumButterFatId, Name = "margarin-maslac-mast",
                ParentCategoryId = KonzumDairyEggsId,
                EquivalentCategoryId = CategorySeedingData.ButterSubcategoryId
            },
            new() { Id = KonzumSpreadsId, Name = "namazi", ParentCategoryId = KonzumDairyEggsId },

            // Drinks subcategories
            new() { Id = KonzumNonAlcoholicId, Name = "bezalkoholna", ParentCategoryId = KonzumDrinksId },
            new()
            {
                Id = KonzumBeerId, Name = "pivo",
                ParentCategoryId = KonzumDrinksId,
                EquivalentCategoryId = CategorySeedingData.AlcoholDrinksSubcategoryId
            },
            new()
            {
                Id = KonzumWineId, Name = "vino",
                ParentCategoryId = KonzumDrinksId,
                EquivalentCategoryId = CategorySeedingData.AlcoholDrinksSubcategoryId
            },
            new()
            {
                Id = KonzumAlcoholId, Name = "alkoholna-pica",
                ParentCategoryId = KonzumDrinksId,
                EquivalentCategoryId = CategorySeedingData.AlcoholDrinksSubcategoryId
            },

            // Sub-subcategories under "bezalkoholna"
            new()
            {
                Id = KonzumWaterId, Name = "voda",
                ParentCategoryId = KonzumNonAlcoholicId,
                EquivalentCategoryId = CategorySeedingData.WaterCategoryId
            },
            new()
            {
                Id = KonzumCarbonatedId, Name = "gazirana",
                ParentCategoryId = KonzumNonAlcoholicId,
                EquivalentCategoryId = CategorySeedingData.CarbonatedDrinksSubcategoryId
            },
            new() { Id = KonzumStillId, Name = "negazirana", ParentCategoryId = KonzumNonAlcoholicId },
            new() { Id = KonzumEnergyId, Name = "energetska-pica", ParentCategoryId = KonzumNonAlcoholicId },

            /*
             * Tommy categories
             */
            // Top categories
            new() { Id = TommyDrinksId, Name = "pica", StoreId = StoreSeedingData.TommyStoreId },
            new()
            {
                Id = TommyDairyDrinksId, Name = "mlijecni-proizvodi-i-jaja", StoreId = StoreSeedingData.TommyStoreId
            },

            // Drinks subcategories
            new() { Id = TommyDrinksId, Name = "jaja", EquivalentCategoryId = CategorySeedingData.EggsSubcategoryId },
            new()
            {
                Id = TommyDrinksId, Name = "margarin-maslac-mast",
                EquivalentCategoryId = CategorySeedingData.ButterSubcategoryId
            },
            new() { Id = TommyDrinksId, Name = "mlijecni-deserti" },
            new() { Id = TommyDrinksId, Name = "vrhnje" },
            new() { Id = TommyDrinksId, Name = "namazi" },
            new()
            {
                Id = TommyDrinksId, Name = "mlijeko", EquivalentCategoryId = CategorySeedingData.MilkSubcategoryId
            },
            new()
            {
                Id = TommyDrinksId, Name = "jogurti-i-ostalo",
                EquivalentCategoryId = CategorySeedingData.YogurtSubcategoryId
            },
            new() { Id = TommyDrinksId, Name = "ledena-kava" },
            new() { Id = TommyDrinksId, Name = "napitci" },
            
            // Drinks subcategories
            new() { Id = TommyNonAlcoholicId, Name = "bezalkoholna", ParentCategoryId = TommyDrinksId },
            new()
            {
                Id = TommyBeerId, Name = "pivo",
                ParentCategoryId = TommyDrinksId,
                EquivalentCategoryId = CategorySeedingData.AlcoholDrinksSubcategoryId
            },
            new()
            {
                Id = TommyWineId, Name = "vino",
                ParentCategoryId = TommyDrinksId,
                EquivalentCategoryId = CategorySeedingData.AlcoholDrinksSubcategoryId
            },
            new()
            {
                Id = TommyStrongAlcoholId, Name = "jaka-alkoholna-pica",
                ParentCategoryId = TommyDrinksId,
                EquivalentCategoryId = CategorySeedingData.AlcoholDrinksSubcategoryId
            },

            // Sub-subcategories under "bezalkoholna"
            new()
            {
                Id = TommyWaterId, Name = "voda",
                ParentCategoryId = TommyNonAlcoholicId,
                EquivalentCategoryId = CategorySeedingData.WaterCategoryId
            },
            new()
            {
                Id = TommyCarbonatedId, Name = "gazirana-pica",
                ParentCategoryId = TommyNonAlcoholicId,
                EquivalentCategoryId = CategorySeedingData.CarbonatedDrinksSubcategoryId
            },
            new() { Id = TommyStillId, Name = "negazirana-pica", ParentCategoryId = TommyNonAlcoholicId },
            new() { Id = TommyEnergyId, Name = "energetska-pica", ParentCategoryId = TommyNonAlcoholicId },
        };
    }
}