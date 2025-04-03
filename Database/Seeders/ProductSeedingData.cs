using PravaCijena.Api.Helpers;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Seeders;

public class ProductSeedingData : CategorySeedingData
{
    public static IEnumerable<Product> InitialProducts()
    {
        return new List<Product>
        {
            /*
             * Milk products
             */
            new()
            {
                Id = new Guid("04133ce5-7349-443d-afb6-83b6a4dc99ed"),
                Name = "Dukat Svježe mlijeko 3,2% m.m. 1 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = new Guid("695f806c-048e-4858-a49a-bfd8c34932af"),
                Name = "Z bregov Svježe mlijeko 3,2% m.m. 1,75 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = new Guid("839cf81b-29d6-4397-8244-aaed755428ca"),
                Name = "Dukat Lagano jutro Trajno mlijeko bez laktoze 1,5% m.m 1 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = new Guid("90be72d4-0f10-41e7-9e95-944fbcebc898"),
                Name = "K Plus Trajno mlijeko 2,8% m.m. 1 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = new Guid("a6d6f654-2551-41ea-af85-f9b976852d20"),
                Name = "Z bregov Mlijeko bez laktoze 2,8% m.m. 1 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = new Guid("4cc38c19-b4da-4708-a582-dc1852dacb30"),
                Name = "Belje Kravica Kraljica Trajno mlijeko 2,8% m.m. 1 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = new Guid("faf662d0-7006-41a7-9000-96895f8912a4"),
                Name = "Dukat Trajno mlijeko 2,8% m.m. 1 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = new Guid("a3e14b35-f6d5-4f6c-b0f4-18fa798a0e9d"),
                Name = "Alpsko Trajno mlijeko 3,5% m.m. 1 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = new Guid("0f2b8014-346d-4585-a68d-a2683e9f7ace"),
                Name = "Z bregov Trajno mlijeko 0,9% m.m. 1 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = new Guid("46e4e5b5-1ac4-4c56-a816-424757676beb"),
                Name = " Z bregov Svježe mlijeko 3,2% m.m. 1 l",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = MilkSubcategoryId
            },
            
            /*
             * Yogurt products
             */
            new()
            {
                Id = new Guid("7f8aadfd-1c53-4f68-b6f6-216091111261"),
                Name = "b Aktiv LGG jogurt 2,4% m.m. 1 kg",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = YogurtSubcategoryId
            },
            new()
            {
                Id = new Guid("7bc71537-30ec-4122-b44b-73d1cf9da71c"),
                Name = "Dukat Tekući jogurt 2,8% m.m. 1 kg",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = YogurtSubcategoryId
            },
            new()
            {
                Id = new Guid("559388ed-55f8-4a0d-bd49-76615bda30ab"),
                Name = "Dukatos Grčki tip jogurta 150 g",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = YogurtSubcategoryId
            },
            new()
            {
                Id = new Guid("f5efe512-7e4b-4fe6-9a7c-4001b70b945e"),
                Name = "b Aktiv LGG Jogurt 2,4% m.m. natur 330 g",
                ImageUrl = "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg",
                CategoryId = YogurtSubcategoryId
            }

            // Add more products as needed
        };
    }
}