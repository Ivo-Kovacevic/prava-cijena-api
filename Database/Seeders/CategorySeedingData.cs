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
    public static readonly Guid MilkDesertsSubcategoryId = new("ba5bf36f-4c5f-4157-a3f9-0ad5594d3269");
    public static readonly Guid OtherMilkSubcategoryId = new("7fd1fe9f-bb7f-402f-8d82-ba4db00d9c44");

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
    public static readonly Guid OtherDrinksSubcategoryId = new("4f190ae8-f2e8-4253-813b-6cd081190c45");

    // Snacks and sweets
    public static readonly Guid SnacksAndSweetsCategoryId = new("95b2126b-0d5b-42b3-9295-3bc57e2c9ebb");
    public static readonly Guid ChocolateSubcategoryId = new("a1eb9a4c-fedb-468e-84b7-3727cac98a3b");
    public static readonly Guid ChipsAndOtherSnacksSubcategoryId = new("e26aa2b9-8b65-44b7-a01a-4569518e5d44");
    public static readonly Guid CandiesAndOtherSweetsSubcategoryId = new("b8b4fcca-2df9-4e73-bb2b-1a6e3b45dac3");
    public static readonly Guid CookiesSubcategoryId = new("23ba58da-3f31-491c-90b2-352c6f8ec551");

    // Basic groceries
    public static readonly Guid BasicGroceriesCategoryId = new("abe37280-7738-4634-9c4e-ca8c0249bb1f");
    public static readonly Guid SoupsSubcategoryId = new("26faf9f4-6b57-4567-a760-14ac644fbd22");
    public static readonly Guid SpreadsSubcategoryId = new("0d4667bc-9675-4e55-8d3c-6f4763f053b2");
    public static readonly Guid PastaSubcategoryId = new("18edbad7-1d0f-413c-9328-dca7cadcc735");
    public static readonly Guid RiceSubcategoryId = new("7e1191aa-8996-4319-b7de-aa0c3d596077");
    public static readonly Guid CerealSubcategoryId = new("f1067e76-70c1-402b-8903-af51b0cab055");

    // Meat
    public static readonly Guid MeatCategoryId = new("bc7764da-28d6-4792-864e-78cbbd72d870");
    public static readonly Guid PorkSubcategoryId = new("e16a2972-b640-432c-a13f-6d23d259e66c");
    public static readonly Guid BeefSubcategoryId = new("2277e21f-087c-41b0-b006-193f7108e8f6");
    public static readonly Guid SeaProductsSubcategoryId = new("6394a258-80ed-4247-8d51-d5594ebdaa43");
    public static readonly Guid PoultrySubcategoryId = new("4a6a6052-12b7-4d2e-ae92-6b10694164a2");
    public static readonly Guid OtherMeatSubcategoryId = new("a6dce7f8-44ce-4575-b03b-cdbfe1f53d10");

    // Bakery
    public static readonly Guid BakeryCategoryId = new("f1b968d4-f39d-456d-828e-32167bf96b79");
    public static readonly Guid BreadSubcategoryId = new("3624d22d-ddda-49ec-8a47-9a707bb61b83");
    public static readonly Guid CakesSubcategoryId = new("87f81cfa-a03c-4101-983c-443c3ee275a4");

    // Hygiene
    public static readonly Guid HygieneCategoryId = new("b20e29d1-1976-4430-8dd9-070ed6b6cfb0");
    public static readonly Guid ShowerSubcategoryId = new("ad811202-c9b5-46e5-bd8c-843262172ebf");
    public static readonly Guid OralHygieneSubcategoryId = new("61b774cf-867c-47dd-851f-17005ada3468");
    public static readonly Guid PerfumeSubcategoryId = new("d48c642b-82d8-4107-8cb1-ae6c9c018f16");
    public static readonly Guid BabyHygieneSubcategoryId = new("94946429-a07d-43c3-a81b-68140b2ea013");
    public static readonly Guid ShaveSubcategoryId = new("3d339b88-726b-40da-87b7-42b8bbf4dfa3");
    public static readonly Guid HairSkinAndOtherCareSubcategoryId = new("0fa350b5-e0ca-4d2c-9e11-844142533f75");
    public static readonly Guid ToiletPaperSubcategoryId = new("1c46faa7-043e-41b1-b59c-d14bd2cac63d");
    public static readonly Guid CottonSubcategoryId = new("fed20933-675f-4ba5-90be-88d0a658b8a0");

    // Cooking
    public static readonly Guid CookingCategoryId = new("13145362-a803-4cc8-92e9-6cea41d5a625");
    public static readonly Guid FlourSubcategoryId = new("fa502e59-6a2d-48f1-acc7-fdccfeb16ae5");
    public static readonly Guid SaltAndOtherSpicesSubcategoryId = new("f8b6a2a8-d316-4950-90a3-cdf6ffbca37d");
    public static readonly Guid CakePreparationSubcategoryId = new("57b681c6-6fe7-463a-9492-bfd0b0b6ed22");
    public static readonly Guid OilSubcategoryId = new("f5a488b4-f094-40b3-ba81-8804731d39fa");
    public static readonly Guid KetchapMayoAndOtherSaucesSubcategoryId = new("0fb670a6-7b34-48ec-bcb1-bf3894dece71");
    public static readonly Guid OtherCookingIngredientsSubcategoryId = new("21d88cfa-a2b4-475a-8bb8-292363affdde");

    // Coffee and tea
    public static readonly Guid CoffeeAndTeaCategoryId = new("afa4406d-a9c7-4ce2-b199-1842879c6d47");
    public static readonly Guid CoffeeSubcategoryId = new("a0737f42-9d31-414b-9506-7d5f2a961118");
    public static readonly Guid TeaSubcategoryId = new("086a1be9-6277-4efd-8aec-2215e807e9fd");

    // Cleaning
    public static readonly Guid CleaningCategoryId = new("61e3e501-79a0-4a6b-b871-4179408237dd");
    public static readonly Guid ClothesCleaningSubcategoryId = new("1f3acc5b-f455-45fb-81f1-d91bc9c00e79");
    public static readonly Guid DishesCleaningSubcategoryId = new("8a3f5a33-d9ef-41ee-9d04-2a2b2df001f5");
    public static readonly Guid SurfaceCleaningSubcategoryId = new("ff25f11d-3498-44f6-aa05-bba92c782609");

    // Ready meals
    public static readonly Guid ReadyMealsCategoryId = new("2bf0ab0c-6c0b-4783-99ea-e19b3be1a5af");
    public static readonly Guid PizzaSubcategoryId = new("aa0174cf-601d-4df4-8dc5-0d98e8047d72");
    public static readonly Guid CannedFoodSubcategoryId = new("fcbc1b31-73a5-4a59-897e-5be37b0567cd");

    // Frozen food
    public static readonly Guid FrozenFoodCategoryId = new("bfba9f9e-4040-42d8-ab00-df46aa0935b9");

    // Other groceries
    public static readonly Guid OtherCategoryId = new("2c884ce7-7df9-4b24-a579-28493e62c5da");

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
                Id = MilkDesertsSubcategoryId,
                Name = "Sladoledi i ostali mliječni deserti",
                ParentCategoryId = DairyCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/milk-desserts.svg",
                HexColor = "#1E71B8"
            },
            new()
            {
                Id = OtherMilkSubcategoryId,
                Name = "Ostali mliječni proizvodi",
                ParentCategoryId = DairyCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/fridge.svg",
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
            new()
            {
                Id = OtherDrinksSubcategoryId,
                Name = "Ostala pića",
                ParentCategoryId = BeveragesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/other-drinks.svg",
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
            },

            /*
             * Basic groceries
             */
            new()
            {
                Id = BasicGroceriesCategoryId,
                Name = "Osnovne namirnice",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/soup-canned.svg",
                HexColor = "#BFBF07"
            },
            new()
            {
                Id = SoupsSubcategoryId,
                Name = "Juhe",
                ParentCategoryId = BasicGroceriesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/soup.svg",
                HexColor = "#BFBF07"
            },
            new()
            {
                Id = SpreadsSubcategoryId,
                Name = "Sirni, čokoladni i ostali namazi",
                ParentCategoryId = BasicGroceriesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/milk-spreads.svg",
                HexColor = "#BFBF07"
            },
            new()
            {
                Id = PastaSubcategoryId,
                Name = "Špagete, makaroni i ostala tjestenina",
                ParentCategoryId = BasicGroceriesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/pasta.svg",
                HexColor = "#BFBF07"
            },
            new()
            {
                Id = RiceSubcategoryId,
                Name = "Riža",
                ParentCategoryId = BasicGroceriesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/rice.svg",
                HexColor = "#BFBF07"
            },
            new()
            {
                Id = CerealSubcategoryId,
                Name = "Žitarice, pahuljice i ostale namirnice",
                ParentCategoryId = BasicGroceriesCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/cereal.svg",
                HexColor = "#BFBF07"
            },


            /*
             * Meat
             */
            new()
            {
                Id = MeatCategoryId,
                Name = "Mesni proizvodi",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/meat.svg",
                HexColor = "#B54848"
            },
            new()
            {
                Id = PorkSubcategoryId,
                Name = "Svinjetina",
                ParentCategoryId = MeatCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/pork.svg",
                HexColor = "#B54848"
            },
            new()
            {
                Id = BeefSubcategoryId,
                Name = "Govedina",
                ParentCategoryId = MeatCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/beef.svg",
                HexColor = "#B54848"
            },
            new()
            {
                Id = SeaProductsSubcategoryId,
                Name = "Riba i morski plodovi",
                ParentCategoryId = MeatCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/fish.svg",
                HexColor = "#B54848"
            },
            new()
            {
                Id = PoultrySubcategoryId,
                Name = "Perad",
                ParentCategoryId = MeatCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/poultry.svg",
                HexColor = "#B54848"
            },
            new()
            {
                Id = OtherMeatSubcategoryId,
                Name = "Delikatese i ostali mesni proizvodi",
                ParentCategoryId = MeatCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/misc-meat.svg",
                HexColor = "#B54848"
            },

            /*
             * Bakery
             */
            new()
            {
                Id = BakeryCategoryId,
                Name = "Pekarnica",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/bakery.svg",
                HexColor = "#E6C34F"
            },
            new()
            {
                Id = BreadSubcategoryId,
                Name = "Kruh, peciva i ostali pekarski proizvodi",
                ParentCategoryId = BakeryCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/bread.svg",
                HexColor = "#E6C34F"
            },
            new()
            {
                Id = CakesSubcategoryId,
                Name = "Torte i kolači",
                ParentCategoryId = BakeryCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/cake.svg",
                HexColor = "#E6C34F"
            },

            /*
             * Hygiene
             */
            new()
            {
                Id = HygieneCategoryId,
                Name = "Njega i higijena",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/hygiene.svg",
                HexColor = "#219DA6"
            },
            new()
            {
                Id = ShowerSubcategoryId,
                Name = "Tuširanje, kupanje, pranje ruku",
                ParentCategoryId = HygieneCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/shower.svg",
                HexColor = "#219DA6"
            },
            new()
            {
                Id = HairSkinAndOtherCareSubcategoryId,
                Name = "Njega kose, kože i ostalo",
                ParentCategoryId = HygieneCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/misc-hygiene.svg",
                HexColor = "#219DA6"
            },
            new()
            {
                Id = OralHygieneSubcategoryId,
                Name = "Oralna higijena",
                ParentCategoryId = HygieneCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/oral-hygiene.svg",
                HexColor = "#219DA6"
            },
            new()
            {
                Id = PerfumeSubcategoryId,
                Name = "Parfemi, dezodoransi i ostali mirisi",
                ParentCategoryId = HygieneCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/perfume.svg",
                HexColor = "#219DA6"
            },
            new()
            {
                Id = BabyHygieneSubcategoryId,
                Name = "Pelene i dječije potrepštine",
                ParentCategoryId = HygieneCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/baby-diaper.svg",
                HexColor = "#219DA6"
            },
            new()
            {
                Id = ShaveSubcategoryId,
                Name = "Brijanje i depilacija",
                ParentCategoryId = HygieneCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/shaver.svg",
                HexColor = "#219DA6"
            },
            new()
            {
                Id = ToiletPaperSubcategoryId,
                Name = "Toaletni papir, ubrusi, vlažne i ostale maramice",
                ParentCategoryId = HygieneCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/toilet-paper.svg",
                HexColor = "#219DA6"
            },
            new()
            {
                Id = CottonSubcategoryId,
                Name = "Ulošci, flasteri i ostale higijenske potrepštine",
                ParentCategoryId = HygieneCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/cotton.svg",
                HexColor = "#219DA6"
            },

            /*
             * Cooking
             */
            new()
            {
                Id = CookingCategoryId,
                Name = "Priprema jela",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/cooking.svg",
                HexColor = "#C28E01"
            },
            new()
            {
                Id = FlourSubcategoryId,
                Name = "Brašno, krušne mrvice, tijesta",
                ParentCategoryId = CookingCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/flour.svg",
                HexColor = "#C28E01"
            },
            new()
            {
                Id = OilSubcategoryId,
                Name = "Ulje",
                ParentCategoryId = CookingCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/oil.svg",
                HexColor = "#C28E01"
            },
            new()
            {
                Id = KetchapMayoAndOtherSaucesSubcategoryId,
                Name = "Kečap, majoneza i ostali umaci",
                ParentCategoryId = CookingCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/sauce.svg",
                HexColor = "#C28E01"
            },
            new()
            {
                Id = SaltAndOtherSpicesSubcategoryId,
                Name = "Sol i ostali začini",
                ParentCategoryId = CookingCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/salt-spices.svg",
                HexColor = "#C28E01"
            },
            new()
            {
                Id = CakePreparationSubcategoryId,
                Name = "Šećer i priprema kolača",
                ParentCategoryId = CookingCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/cake-preparation.svg",
                HexColor = "#C28E01"
            },
            new()
            {
                Id = OtherCookingIngredientsSubcategoryId,
                Name = "Ostale namirnice za kuhanje",
                ParentCategoryId = CookingCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/misc-cooking.svg",
                HexColor = "#C28E01"
            },

            /*
             * Coffee and tea
             */
            new()
            {
                Id = CoffeeAndTeaCategoryId,
                Name = "Kava i čaj",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/coffee-tea.svg",
                HexColor = "#8B623A"
            },
            new()
            {
                Id = CoffeeSubcategoryId,
                Name = "Kava",
                ParentCategoryId = CoffeeAndTeaCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/coffee.svg",
                HexColor = "#8B623A"
            },
            new()
            {
                Id = TeaSubcategoryId,
                Name = "Čajevi",
                ParentCategoryId = CoffeeAndTeaCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/tea.svg",
                HexColor = "#8B623A"
            },

            /*
             * Frozen
             */
            new()
            {
                Id = FrozenFoodCategoryId,
                Name = "Smrznuta hrana",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/frozen.svg",
                HexColor = "#0840B9"
            },

            /*
             * Ready meals
             */
            new()
            {
                Id = ReadyMealsCategoryId,
                Name = "Gotova jela",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/ready-meals.svg",
                HexColor = "#C02222"
            },
            new()
            {
                Id = PizzaSubcategoryId,
                Name = "Pizza",
                ParentCategoryId = ReadyMealsCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/pizza.svg",
                HexColor = "#C02222"
            },
            new()
            {
                Id = CannedFoodSubcategoryId,
                Name = "Konzervirana hrana",
                ParentCategoryId = ReadyMealsCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/canned.svg",
                HexColor = "#C02222"
            },

            /*
             * Cleaning
             */
            new()
            {
                Id = CleaningCategoryId,
                Name = "Sredstva za pranje i čišćenje",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/cleaning.svg",
                HexColor = "#995BCB"
            },
            new()
            {
                Id = ClothesCleaningSubcategoryId,
                Name = "Pranje odjeće",
                ParentCategoryId = CleaningCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/clothes.svg",
                HexColor = "#995BCB"
            },
            new()
            {
                Id = DishesCleaningSubcategoryId,
                Name = "Pranje posuđa",
                ParentCategoryId = CleaningCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/dishes.svg",
                HexColor = "#995BCB"
            },
            new()
            {
                Id = SurfaceCleaningSubcategoryId,
                Name = "Sredstva za pranje kupaonice i ostalih površina",
                ParentCategoryId = CleaningCategoryId,
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/surface-cleaning.svg",
                HexColor = "#995BCB"
            },

            /*
             * Other groceries
             */
            new()
            {
                Id = OtherCategoryId,
                Name = "Ostale namirnice i proizvodi",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/misc.svg",
                HexColor = "#787878"
            }
        };
    }
}