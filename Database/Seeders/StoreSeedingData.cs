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
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/konzum.webp",

                PriceListUrl = "https://www.konzum.hr/cjenici",
                PriceUrlListXPath = "//div[@class='col-8 col-md-9 col-lg-10']",
                PriceUrlXPath = ".//h5//a",
                CsvDelimiter = ",",
                CsvNameColumn = 0,
                CsvBrandColumn = 2,
                CsvPriceColumn = 5,
                CsvBarcodeColumn = 10,

                BaseCategoryUrl = "https://www.konzum.hr/web/t/kategorije",
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
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/lidl.webp",

                PriceListUrl = "https://tvrtka.lidl.hr/cijene",
                PriceUrlListXPath = "//div[@class='landing-page__zone landing-page__zone--262536']",
                PriceUrlXPath = ".//a",
                CsvDelimiter = "\t",
                CsvNameColumn = 0,
                CsvBrandColumn = 4,
                CsvPriceColumn = 5,
                CsvBarcodeColumn = 9
            },
            new()
            {
                Id = StudenacStoreId,
                Name = "Studenac",
                StoreUrl = "https://www.studenac.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/studenac.webp",

                PriceListUrl = "https://www.studenac.hr/popis-maloprodajnih-cijena",
                PriceUrlListXPath = "//div[@class='page__cta']",
                PriceUrlXPath = ".//a[@class='btn btn--big btn--yellow']",
                XmlNameElement = "NazivProizvoda",
                XmlBrandElement = "MarkaProizvoda",
                XmlPriceElement = "MaloprodajnaCijena",
                XmlBarcodeElement = "Barkod",

                CatalogueListUrl = "https://www.plodine.hr/aktualni-katalozi",
                CatalogueListXPath = "//ul[@class='cataloguelisting__list']//div[@class='card__cta']"
            },
            new()
            {
                Id = TommyStoreId,
                Name = "Tommy",
                StoreUrl = "https://www.tommy.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/tommy.webp",

                PriceListUrl = "https://www.tommy.hr/objava-cjenika",

                BaseCategoryUrl = "https://www.tommy.hr/kategorije",
                ProductListXPath = "//section//article",
                PageQuery = "page",
                LimitQuery = "limit"
            },
            new()
            {
                Id = SparStoreId,
                Name = "Spar",
                StoreUrl = "https://www.spar.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/spar.webp",

                PriceListUrl = "https://www.spar.hr/usluge/cjenici",
                PriceUrlListXPath = "//table[@id='fileTable']",
                PriceUrlXPath = ".//a",
                CsvDelimiter = ";",
                CsvNameColumn = 0,
                CsvBrandColumn = 2,
                CsvPriceColumn = 5,
                CsvBarcodeColumn = 10
            },
            new()
            {
                Id = EurospinStoreId,
                Name = "Eurospin",
                StoreUrl = "https://www.eurospin.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/eurospin.webp",

                PriceListUrl = "https://www.eurospin.hr/cjenik/",
                PriceUrlListXPath = "//div[@class='pdf-grid ']"
            },
            new()
            {
                Id = KauflandStoreId,
                Name = "Kaufland",
                StoreUrl = "https://www.kaufland.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/kaufland.webp",

                PriceListUrl = "https://www.kaufland.hr/akcije-novosti/mpc-popis.html",
                PriceUrlListXPath = "//div[@class='accordion2 abstractcomponent']",

                CatalogueListUrl = "https://www.kaufland.hr/aktualni-katalozi.html",
                CatalogueListXPath =
                    "//div[@class='m-tab-navigation__inner-container m-tab-navigation__inner-container--show']//div[@class='m-flyer-tile ']"
            },
            new()
            {
                Id = PlodineStoreId,
                Name = "Plodine",
                StoreUrl = "https://www.plodine.hr",
                ImageUrl = "https://res.cloudinary.com/dqbe0apqn/image/upload/plodine.webp",

                PriceListUrl = "https://www.plodine.hr/info-o-cijenama",
                PriceUrlListXPath = "//ul[@class='accordion']//a",
                PriceUrlXPath = ".//a",
                CsvDelimiter = ";",
                CsvNameColumn = 0,
                CsvBrandColumn = 2,
                CsvPriceColumn = 5,
                CsvBarcodeColumn = 10,

                CatalogueListUrl = "https://www.plodine.hr/aktualni-katalozi",
                CatalogueListXPath = "//div[@class='catalog__wrap']//a[@class='btn btn--iconR']"
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