namespace PravaCijena.Api.Database.Seeders;

public class ProductValuesSeedingData
{
    // public static IEnumerable<ProductValue> InitialProductValues()
    // {
    //     return new List<ProductValue>
    //     {
    //         // Dukat Svježe mlijeko 3,2% m.m. 1 l
    //         new()
    //         {
    //             Id = Guid.Parse("32adf6a2-b2f9-4a2f-96b1-60d0d8a1e401"),
    //             ProductId = ProductSeedingData.DukatSvjezeMlijeko32_1L,
    //             ValueId = ValueSeedingData.MilkProducerValueDukat
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("e86ae7c6-35a2-4d88-8b1e-1c82e050878f"),
    //             ProductId = ProductSeedingData.DukatSvjezeMlijeko32_1L,
    //             ValueId = ValueSeedingData.MilkLifeValueFresh
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("ad016c9d-fc6f-4a1d-91d4-cf1fdb48b611"),
    //             ProductId = ProductSeedingData.DukatSvjezeMlijeko32_1L,
    //             ValueId = ValueSeedingData.MilkFatValueMoreThan3
    //         },
    //
    //         // Z bregov Svježe mlijeko 3,2% m.m. 1,75 l
    //         new()
    //         {
    //             Id = Guid.Parse("3348d7e5-37db-4439-82d6-b1c71b8d06cd"),
    //             ProductId = ProductSeedingData.ZBregovSvjezeMlijeko32_175L,
    //             ValueId = ValueSeedingData.MilkProducerValueZbregov
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("cce8cba1-7fc6-4ae5-8dc0-4ccca6467c61"),
    //             ProductId = ProductSeedingData.ZBregovSvjezeMlijeko32_175L,
    //             ValueId = ValueSeedingData.MilkLifeValueFresh
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("99be2b89-cc61-4e62-aaa5-8f749d1602cf"),
    //             ProductId = ProductSeedingData.ZBregovSvjezeMlijeko32_175L,
    //             ValueId = ValueSeedingData.MilkFatValueMoreThan3
    //         },
    //
    //         // Dukat Lagano jutro bez laktoze 1,5% m.m 1 l
    //         new()
    //         {
    //             Id = Guid.Parse("064e61a4-d85c-41cf-9104-37613be8cb06"),
    //             ProductId = ProductSeedingData.DukatLaganoJutroBezLaktoze15_1L,
    //             ValueId = ValueSeedingData.MilkProducerValueDukat
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("98c5d213-2448-4e01-8492-e3d3734f64ae"),
    //             ProductId = ProductSeedingData.DukatLaganoJutroBezLaktoze15_1L,
    //             ValueId = ValueSeedingData.MilkFatValue1To2
    //         },
    //
    //         // K Plus Trajno mlijeko 2,8% m.m. 1 l
    //         new()
    //         {
    //             Id = Guid.Parse("d4aa8c37-f735-4e7a-a39a-d6c40593708a"),
    //             ProductId = ProductSeedingData.KPlusTrajnoMlijeko28_1L,
    //             ValueId = ValueSeedingData.MilkLifeValueLongLasting
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("d899c542-bcc4-4b3a-88f4-3cbe3bfb8d44"),
    //             ProductId = ProductSeedingData.KPlusTrajnoMlijeko28_1L,
    //             ValueId = ValueSeedingData.MilkFatValue2To3
    //         },
    //
    //         // Z bregov Mlijeko bez laktoze 2,8% m.m. 1 l
    //         new()
    //         {
    //             Id = Guid.Parse("83d147bb-43b7-42ef-8333-4f8e2e6e76c0"),
    //             ProductId = ProductSeedingData.ZBregovBezLaktoze28_1L,
    //             ValueId = ValueSeedingData.MilkProducerValueZbregov
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("e8447c9f-b6b5-4db6-b418-5dbf2f64e2cd"),
    //             ProductId = ProductSeedingData.ZBregovBezLaktoze28_1L,
    //             ValueId = ValueSeedingData.MilkFatValue2To3
    //         },
    //
    //         // Belje Kravica Kraljica Trajno mlijeko 2,8% m.m. 1 l
    //         new()
    //         {
    //             Id = Guid.Parse("bc96ac4d-9341-445e-bab5-70b90f49fadd"),
    //             ProductId = ProductSeedingData.BeljeKravicaKraljicaTrajno28_1L,
    //             ValueId = ValueSeedingData.MilkProducerValueBelje
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("f1148b77-d8b4-4c7b-9471-8884d8b83a1c"),
    //             ProductId = ProductSeedingData.BeljeKravicaKraljicaTrajno28_1L,
    //             ValueId = ValueSeedingData.MilkLifeValueLongLasting
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("0c352cf7-e6f5-404c-a81f-78ccceecb84b"),
    //             ProductId = ProductSeedingData.BeljeKravicaKraljicaTrajno28_1L,
    //             ValueId = ValueSeedingData.MilkFatValue2To3
    //         },
    //
    //         // Dukat Trajno mlijeko 2,8% m.m. 1 l
    //         new()
    //         {
    //             Id = Guid.Parse("c3d82cf8-1370-4e35-8d7f-92b77e9a1f50"),
    //             ProductId = ProductSeedingData.DukatTrajnoMlijeko28_1L,
    //             ValueId = ValueSeedingData.MilkProducerValueDukat
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("712ae8f7-6b8b-4b6b-a339-b4b255b2aa6f"),
    //             ProductId = ProductSeedingData.DukatTrajnoMlijeko28_1L,
    //             ValueId = ValueSeedingData.MilkLifeValueLongLasting
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("1b80c584-3e67-4a84-9c17-08e0a6c8584a"),
    //             ProductId = ProductSeedingData.DukatTrajnoMlijeko28_1L,
    //             ValueId = ValueSeedingData.MilkFatValue2To3
    //         },
    //
    //         // Alpsko Trajno mlijeko 3,5% m.m. 1 l
    //         new()
    //         {
    //             Id = Guid.Parse("5d76d7a6-36ad-4344-a0b1-663e1f99f07f"),
    //             ProductId = ProductSeedingData.AlpskoTrajnoMlijeko35_1L,
    //             ValueId = ValueSeedingData.MilkProducerValueAlpsko
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("89185df2-2e3f-4b5a-8430-963e3d0379c3"),
    //             ProductId = ProductSeedingData.AlpskoTrajnoMlijeko35_1L,
    //             ValueId = ValueSeedingData.MilkLifeValueLongLasting
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("a55df7a9-f1fc-4cf0-9497-59d94b7f1864"),
    //             ProductId = ProductSeedingData.AlpskoTrajnoMlijeko35_1L,
    //             ValueId = ValueSeedingData.MilkFatValueMoreThan3
    //         },
    //
    //         // Z bregov Trajno mlijeko 0,9% m.m. 1 l
    //         new()
    //         {
    //             Id = Guid.Parse("5cf63e8c-9887-4f8a-baf9-bcf8ff6f31a3"),
    //             ProductId = ProductSeedingData.ZBregovTrajnoMlijeko09_1L,
    //             ValueId = ValueSeedingData.MilkProducerValueZbregov
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("91f8e1cf-8a13-4f92-9a26-51e0fcf8384a"),
    //             ProductId = ProductSeedingData.ZBregovTrajnoMlijeko09_1L,
    //             ValueId = ValueSeedingData.MilkLifeValueLongLasting
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("d33288d2-6ed4-4e18-998d-d0f0a3fa39f1"),
    //             ProductId = ProductSeedingData.ZBregovTrajnoMlijeko09_1L,
    //             ValueId = ValueSeedingData.MilkFatValueLessThan1
    //         },
    //
    //         // Z bregov Svježe mlijeko 3,2% m.m. 1 l
    //         new()
    //         {
    //             Id = Guid.Parse("35ad2684-028c-4ce6-a6cb-4f4a6eb729d0"),
    //             ProductId = ProductSeedingData.ZBregovSvjezeMlijeko32_1L,
    //             ValueId = ValueSeedingData.MilkProducerValueZbregov
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("58e26446-c648-4a4b-bddb-5aa7486a2631"),
    //             ProductId = ProductSeedingData.ZBregovSvjezeMlijeko32_1L,
    //             ValueId = ValueSeedingData.MilkLifeValueFresh
    //         },
    //         new()
    //         {
    //             Id = Guid.Parse("44b3aeb0-40a1-4720-8918-8fd3cd0e6b88"),
    //             ProductId = ProductSeedingData.ZBregovSvjezeMlijeko32_1L,
    //             ValueId = ValueSeedingData.MilkFatValueMoreThan3
    //         }
    //     };
    // }
}