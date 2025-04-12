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
    public static readonly Guid MilkSpreadsSubcategoryId = new("0d4667bc-9675-4e55-8d3c-6f4763f053b2");
    public static readonly Guid MilkDesertsSubcategoryId = new("ba5bf36f-4c5f-4157-a3f9-0ad5594d3269");

    // Fruits and vegetables category
    public static readonly Guid FruitsAndVegetablesCategoryId = new("f91a4d2c-dbe7-42ad-a3cd-7d2a0f557ec6");
    public static readonly Guid FruitsSubcategoryId = new("12c593c9-2fc2-4ef6-b537-9ed6a95f2e96");
    public static readonly Guid VegetablesSubcategoryId = new("1ac3b0d3-d60e-4979-8886-94b43f194e28");
    public static readonly Guid FrozenFruitsAndVegetablesSubcategoryId = new("742ff5e3-92e6-4a89-8d77-d49a4e92065c");
    public static readonly Guid DryFruitsAndVegetablesSubcategoryId = new("5a16ab3e-05ae-4679-9d96-77f0db2c47a3");

    // Beverages category
    public static readonly Guid BeveragesCategoryId = new("91ac1be2-b97c-47ed-902d-712a96d8b0f0");
    public static readonly Guid WaterCategoryId = new("320547d7-17e5-41d3-960f-32839cf62be6");
    public static readonly Guid CarbonatedDrinksSubcategoryId = new("7f4d2586-e464-4b18-9e4f-d4b38c553295");
    public static readonly Guid NonCarbonatedSubcategoryId = new("5de62c74-fb47-468a-b56d-5d9d38208039");
    public static readonly Guid AlcoholDrinksSubcategoryId = new("a3f95625-4ec1-4222-8cdc-b491c4356f9c");

    // Snacks and sweets
    public static readonly Guid SnacksAndSweetsCategoryId = new("95b2126b-0d5b-42b3-9295-3bc57e2c9ebb");
    public static readonly Guid ChocolateSubcategoryId = new("a1eb9a4c-fedb-468e-84b7-3727cac98a3b");
    public static readonly Guid ChipsAndOtherSnacksSubcategoryId = new("e26aa2b9-8b65-44b7-a01a-4569518e5d44");
    public static readonly Guid CandiesAndOtherSweetsSubcategoryId = new("b8b4fcca-2df9-4e73-bb2b-1a6e3b45dac3");
    public static readonly Guid CookiesSubcategoryId = new("23ba58da-3f31-491c-90b2-352c6f8ec551");

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
                Name = "Mliječni proizvodi i jaja",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/dairy.svg",
                HexColor = "#1E71B8"
            },
            new()
            {
                Id = MilkSubcategoryId,
                Name = "Mlijeko",
                ParentCategoryId = DairyCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/milk.svg",
                HexColor = "#1E71B8"
            },
            new()
            {
                Id = EggsSubcategoryId,
                Name = "Jaja",
                ParentCategoryId = DairyCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/eggs.svg",
                HexColor = "#1E71B8"
            },
            new()
            {
                Id = CheeseSubcategoryId,
                Name = "Sirevi",
                ParentCategoryId = DairyCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/cheese.svg",
                HexColor = "#1E71B8"
            },
            new()
            {
                Id = YogurtSubcategoryId,
                Name = "Jogurti, kefiri, vrhnja",
                ParentCategoryId = DairyCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/yogurt.svg",
                HexColor = "#1E71B8"
            },
            new()
            {
                Id = ButterSubcategoryId,
                Name = "Maslac, margarin, mast",
                ParentCategoryId = DairyCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/butter.svg",
                HexColor = "#1E71B8"
            },
            new()
            {
                Id = MilkSpreadsSubcategoryId,
                Name = "Mliječni namazi",
                ParentCategoryId = DairyCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/milk-spreads.svg",
                HexColor = "#1E71B8"
            },
            new()
            {
                Id = MilkDesertsSubcategoryId,
                Name = "Mliječni deserti",
                ParentCategoryId = DairyCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/milk-desserts.svg",
                HexColor = "#1E71B8"
            },

            /*
             * Fruits and vegetables category and subcategories
             */
            new()
            {
                Id = FruitsAndVegetablesCategoryId,
                Name = "Voće i povrće",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/fruits-and-vegetables.svg",
                HexColor = "#60A158"
            },
            new()
            {
                Id = FruitsSubcategoryId,
                Name = "Voće",
                ParentCategoryId = FruitsAndVegetablesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/fruits.svg",
                HexColor = "#60A158"
            },
            new()
            {
                Id = VegetablesSubcategoryId,
                Name = "Povrće",
                ParentCategoryId = FruitsAndVegetablesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/vegetables.svg",
                HexColor = "#60A158"
            },
            new()
            {
                Id = FrozenFruitsAndVegetablesSubcategoryId,
                Name = "Smrznuto voće i povrće",
                ParentCategoryId = FruitsAndVegetablesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/frozen-fruits-and-vegetables.svg",
                HexColor = "#60A158"
            },
            new()
            {
                Id = DryFruitsAndVegetablesSubcategoryId,
                Name = "Suho voće i povrće, orašasto, sjemenke",
                ParentCategoryId = FruitsAndVegetablesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/dry-fruits-and-vegetables.svg",
                HexColor = "#60A158"
            },

            /*
             * Beverages category and subcategories
             */
            new()
            {
                Id = BeveragesCategoryId,
                Name = "Pića",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/beverages.svg",
                HexColor = "#41949E"
            },
            new()
            {
                Id = WaterCategoryId,
                Name = "Voda",
                ParentCategoryId = BeveragesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/water.svg",
                HexColor = "#41949E"
            },
            new()
            {
                Id = CarbonatedDrinksSubcategoryId,
                Name = "Gazirana pića",
                ParentCategoryId = BeveragesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/carbonated-drinks.svg",
                HexColor = "#41949E"
            },

            new()
            {
                Id = NonCarbonatedSubcategoryId,
                Name = "Negazirana pića",
                ParentCategoryId = BeveragesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/non-carbonated.svg",
                HexColor = "#41949E"
            },
            new()
            {
                Id = AlcoholDrinksSubcategoryId,
                Name = "Alkoholna pića",
                ParentCategoryId = BeveragesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/alcohol.svg",
                HexColor = "#41949E"
            },

            /*
             * Snacks and sweets category and subcategories
             */
            new()
            {
                Id = SnacksAndSweetsCategoryId,
                Name = "Grickalice i slatkiši",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/snacks-and-candies.svg",
                HexColor = "#A4539E"
            },
            new()
            {
                Id = ChocolateSubcategoryId,
                Name = "Čokolade, bombonjere, snackovi",
                ParentCategoryId = SnacksAndSweetsCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/chocolate.svg",
                HexColor = "#A4539E"
            },
            new()
            {
                Id = ChipsAndOtherSnacksSubcategoryId,
                Name = "Čips, flips, štapići i ostale grickalice",
                ParentCategoryId = SnacksAndSweetsCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/chips.svg",
                HexColor = "#A4539E"
            },
            new()
            {
                Id = CandiesAndOtherSweetsSubcategoryId,
                Name = "Bomboni, lizalice i ostali slatkiši",
                ParentCategoryId = SnacksAndSweetsCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/candies.svg",
                HexColor = "#A4539E"
            },
            new()
            {
                Id = CookiesSubcategoryId,
                Name = "Keksi",
                ParentCategoryId = SnacksAndSweetsCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/cookies.svg",
                HexColor = "#A4539E"
            }
        };
    }
}