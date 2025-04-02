using api.Models;

namespace api.Database.Seeders;

public class ValueSeedingData : LabelSeedingData
{
    // Milk label values
    public static readonly Guid MilkTypeCowValue = new Guid("2ce02301-83c2-4fb7-8397-185a4ea2e78c");
    public static readonly Guid MilkTypeGoatValue = new Guid("2ce02301-83c2-4fb7-8397-185a4ea2e78d");
    public static readonly Guid MilkLifeValueId1 = new Guid("43b33b76-69cc-4424-8608-eeb20f931477");
    public static readonly Guid MilkLifeValueId2 = new Guid("95e493da-1772-4be5-8038-250a5453c2bb");
    public static readonly Guid MilkFatValueId1 = new Guid("bf131788-50bf-413a-94c2-92c8b1de1adf");
    public static readonly Guid MilkFatValueId2 = new Guid("b93dc567-d6d3-4dae-b3bd-9a4b990a5f6b");
    public static readonly Guid MilkFatValueId3 = new Guid("bb601c25-43f5-403b-8ec1-92c470ec35e5");
    public static readonly Guid MilkFatValueId4 = new Guid("48923d87-1b7e-4121-9f87-023e71712203");
    public static readonly Guid MilkProducerValueId1 = new Guid("d813d858-809f-49e2-8f87-2b69b37b194b");
    public static readonly Guid MilkProducerValueId2 = new Guid("e4006d51-2e28-46be-8747-159956b6fdbb");
    public static readonly Guid MilkProducerValueId3 = new Guid("2a99e355-fcf1-46c8-8b5b-019bdbfc99de");

    public static IEnumerable<Value> InitialValues()
    {
        return new List<Value>
        {
            /*
             * Milk label values
             */
            new()
            {
                Id = MilkTypeCowValue,
                LabelId = MilkTypeLabelId,
                Name = "Kravlje mlijeko"
            },
            new()
            {
                Id = MilkTypeGoatValue,
                LabelId = MilkTypeLabelId,
                Name = "Kozje mlijeko"
            },
            
            new()
            {
                Id = MilkLifeValueId1,
                LabelId = MilkLifeLabelId,
                Name = "Svježe mlijeko"
            },
            new()
            {
                Id = MilkLifeValueId2,
                LabelId = MilkLifeLabelId,
                Name = "Trajno mlijeko"
            },
            
            new()
            {
                Id = MilkFatValueId1,
                LabelId = MilkFatLabelId,
                Name = "do 1%"
            },
            new()
            {
                Id = MilkFatValueId2,
                LabelId = MilkFatLabelId,
                Name = "1% do 2%"
            },
            new()
            {
                Id = MilkFatValueId3,
                LabelId = MilkFatLabelId,
                Name = "2% do 3%"
            },
            new()
            {
                Id = MilkFatValueId4,
                LabelId = MilkFatLabelId,
                Name = "iznad 3%"
            },
            
            new()
            {
                Id = MilkProducerValueId1,
                LabelId = MilkProducerLabelId,
                Name = "Dukat"
            },
            new()
            {
                Id = MilkProducerValueId2,
                LabelId = MilkProducerLabelId,
                Name = "z bregov"
            },
            new()
            {
                Id = MilkProducerValueId3,
                LabelId = MilkProducerLabelId,
                Name = "Moja kravica"
            }
        };
    }
}
