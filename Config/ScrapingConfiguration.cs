using PravaCijena.Api.Models.AutomationModels;

namespace PravaCijena.Api.Config;

public static class ScrapingConfiguration
{
    public static List<StoreScrapingConfig> StoresList =>
    [
        new()
        {
            StoreSlug = "konzum",
            Url = "https://www.konzum.hr/web/t/kategorije",
            ProductListXPath =
                "//div[@class='product-list product-list--md-5 js-product-layout-container product-list--grid']//article",
            Categories =
            [
                new Category("mlijecni-proizvodi-i-jaja", [
                    new Category("mlijeko"),
                    // new Category("sirevi"),
                    // new Category("jaja"),
                    // new Category("jogurti-i-ostalo"),
                    // new Category("mlijecni-deserti"),
                    // new Category("margarin-maslac-mast"),
                    // new Category("namazi")
                ]),
                // new Category("pica", [
                //     new Category("bezalkoholna", [
                //         new Category("voda"),
                //         new Category("gazirana"),
                //         new Category("negazirana"),
                //         new Category("energetska-pica")
                //     ]),
                //     new Category("pivo"),
                //     new Category("vino"),
                //     new Category("alkoholna-pica")
                // ])
            ]
        }
        // new()
        // {
        //     StoreName = "tommy",
        //     Url = "https://www.tommysss.hr/kategorije/",
        //     ProductListXPath =
        //         "//div[@class='@container flex  flex-wrap -mx-1 xs:-mx-2.5 ']//article",
        //     Categories =
        //     [
        //         new Category("mlijecni-proizvodi-i-jaja", [
        //             new Category("jaja"),
        //             new Category("margarin-maslac-mast"),
        //             new Category("sirevi"),
        //             new Category("mlijecni-deserti"),
        //             new Category("vrhnje"),
        //             new Category("namazi"),
        //             new Category("mlijeko"),
        //             new Category("jogurti-i-ostalo"),
        //             new Category("ledena-kava"),
        //             new Category("napitci")
        //         ]),
        //         new Category("pica", [
        //             new Category("bezalkoholna-pica", [
        //                 new Category("gazirana-pica"),
        //                 new Category("voda"),
        //                 new Category("negazirana-pica"),
        //                 new Category("energetska-pica")
        //             ]),
        //             new Category("pivo"),
        //             new Category("vino"),
        //             new Category("jaka-alkoholna-pica")
        //         ])
        //     ]
        // }
    ];
}