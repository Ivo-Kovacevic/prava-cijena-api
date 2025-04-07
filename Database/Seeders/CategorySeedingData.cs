using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Seeders;

public class CategorySeedingData
{
    /// Dairy category
    public static readonly Guid DairyCategoryId = new("f5a5d762-7a8f-4a8e-8f30-d9f4db59b7f0");

    public static readonly Guid MilkSubcategoryId = new("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f");
    public static readonly Guid EggsSubcategoryId = new("7b3a5662-d9f6-48ad-b6ea-6fce8c92715a");
    public static readonly Guid CheeseSubcategoryId = new("aad5b759-df12-4937-a1f5-91c30a0f4e90");
    public static readonly Guid YogurtSubcategoryId = new("aa4c9e5f-3e8b-4d99-a04e-dadfe2a5e10e");
    public static readonly Guid ButterSubcategoryId = new("b625a4f2-533b-4a33-822b-488457325b5d");
    public static readonly Guid IceCreamSubcategoryId = new("0d4667bc-9675-4e55-8d3c-6f4763f053b2");

    // Vegetables category
    public static readonly Guid VegetablesCategoryId = new("f91a4d2c-dbe7-42ad-a3cd-7d2a0f557ec6");

    // Fruits category
    public static readonly Guid FruitsCategoryId = new("12c593c9-2fc2-4ef6-b537-9ed6a95f2e96");

    // Meat category
    public static readonly Guid MeatCategoryId = new("17e63574-d2b8-4a74-b94d-3210fc0b4186");
    public static readonly Guid PorkSubcategoryId = new("e9e34ab3-1bdb-4005-9508-d084071f5850");
    public static readonly Guid PoultrySubcategoryId = new("5a16ab3e-05ae-4679-9d96-77f0db2c47a3");
    public static readonly Guid BeefSubcategoryId = new("742ff5e3-92e6-4a89-8d77-d49a4e92065c");
    public static readonly Guid SeafoodSubcategoryId = new("1ac3b0d3-d60e-4979-8886-94b43f194e28");

    // Beverages category
    public static readonly Guid BeveragesCategoryId = new("91ac1be2-b97c-47ed-902d-712a96d8b0f0");
    public static readonly Guid WaterCategoryId = new("320547d7-17e5-41d3-960f-32839cf62be6");
    public static readonly Guid CoffeeSubcategoryId = new("5de62c74-fb47-468a-b56d-5d9d38208039");
    public static readonly Guid TeaSubcategoryId = new("41da84c7-0196-4bde-8e0c-bd20688e6e63");
    public static readonly Guid CarbonatedDrinksSubcategoryId = new("7f4d2586-e464-4b18-9e4f-d4b38c553295");
    public static readonly Guid AlcoholDrinksSubcategoryId = new("a3f95625-4ec1-4222-8cdc-b491c4356f9c");


    public static IEnumerable<Category> InitialCategories()
    {
        return new List<Category>
        {
            /*
             * Dairy category and subcategories
             */
            new()
            {
                Id = DairyCategoryId,
                Name = "Mliječni proizvodi i jaja"
            },
            new()
            {
                Id = MilkSubcategoryId,
                Name = "Mlijeko",
                ParentCategoryId = DairyCategoryId
            },
            new()
            {
                Id = EggsSubcategoryId,
                Name = "Jaja",
                ParentCategoryId = DairyCategoryId
            },
            new()
            {
                Id = CheeseSubcategoryId,
                Name = "Sirevi",
                ParentCategoryId = DairyCategoryId
            },
            new()
            {
                Id = YogurtSubcategoryId,
                Name = "Jogurti",
                ParentCategoryId = DairyCategoryId
            },
            new()
            {
                Id = ButterSubcategoryId,
                Name = "Maslac",
                ParentCategoryId = DairyCategoryId
            },
            new()
            {
                Id = IceCreamSubcategoryId,
                Name = "Sladoledi",
                ParentCategoryId = DairyCategoryId
            },

            /*
             * Vegetables category and subcategories
             */
            new()
            {
                Id = VegetablesCategoryId,
                Name = "Povrće"
            },

            /*
             * Fruits category and subcategories
             */
            new()
            {
                Id = FruitsCategoryId,
                Name = "Voće"
            },

            /*
             * Meat category and subcategories
             */
            new()
            {
                Id = MeatCategoryId,
                Name = "Meso"
            },
            new()
            {
                Id = PorkSubcategoryId,
                Name = "Svinjsko meso",
                ParentCategoryId = MeatCategoryId
            },
            new()
            {
                Id = PoultrySubcategoryId,
                Name = "Perad",
                ParentCategoryId = MeatCategoryId
            },
            new()
            {
                Id = BeefSubcategoryId,
                Name = "Govedina",
                ParentCategoryId = MeatCategoryId
            },
            new()
            {
                Id = SeafoodSubcategoryId,
                Name = "Plodovi mora",
                ParentCategoryId = MeatCategoryId
            },

            /*
             * Beverages category and subcategories
             */
            new()
            {
                Id = BeveragesCategoryId,
                Name = "Pića"
            },
            new()
            {
                Id = CoffeeSubcategoryId,
                Name = "Kava",
                ParentCategoryId = BeveragesCategoryId
            },
            new()
            {
                Id = WaterCategoryId,
                Name = "Voda",
                ParentCategoryId = BeveragesCategoryId
            },
            new()
            {
                Id = TeaSubcategoryId,
                Name = "Čaj",
                ParentCategoryId = BeveragesCategoryId
            },
            new()
            {
                Id = CarbonatedDrinksSubcategoryId,
                Name = "Gazirana pića",
                ParentCategoryId = BeveragesCategoryId
            },
            new()
            {
                Id = AlcoholDrinksSubcategoryId,
                Name = "Alkoholna pića",
                ParentCategoryId = BeveragesCategoryId
            }
        };
    }
}