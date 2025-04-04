using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Seeders;

public class ValueSeedingData : LabelSeedingData
{
    // Milk label values
    public static readonly Guid MilkTypeCowValue = new("2ce02301-83c2-4fb7-8397-185a4ea2e78c");
    public static readonly Guid MilkTypeGoatValue = new("2ce02301-83c2-4fb7-8397-185a4ea2e78d");
    public static readonly Guid MilkLifeValueFresh = new("43b33b76-69cc-4424-8608-eeb20f931477");
    public static readonly Guid MilkLifeValueLongLasting = new("95e493da-1772-4be5-8038-250a5453c2bb");
    public static readonly Guid MilkFatValueLessThan1 = new("bf131788-50bf-413a-94c2-92c8b1de1adf");
    public static readonly Guid MilkFatValue1To2 = new("b93dc567-d6d3-4dae-b3bd-9a4b990a5f6b");
    public static readonly Guid MilkFatValue2To3 = new("bb601c25-43f5-403b-8ec1-92c470ec35e5");
    public static readonly Guid MilkFatValueMoreThan3 = new("48923d87-1b7e-4121-9f87-023e71712203");
    public static readonly Guid MilkProducerValueDukat = new("d813d858-809f-49e2-8f87-2b69b37b194b");
    public static readonly Guid MilkProducerValueZbregov = new("e4006d51-2e28-46be-8747-159956b6fdbb");
    public static readonly Guid MilkProducerValueAlpsko = new("2a99e355-fcf1-46c8-8b5b-019bdbfc99de");
    public static readonly Guid MilkProducerValueBelje = new("7dc32a5c-84eb-450a-a2fd-8117374e0b24");

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
                Id = MilkLifeValueFresh,
                LabelId = MilkLifeLabelId,
                Name = "Svježe mlijeko"
            },
            new()
            {
                Id = MilkLifeValueLongLasting,
                LabelId = MilkLifeLabelId,
                Name = "Trajno mlijeko"
            },

            new()
            {
                Id = MilkFatValueLessThan1,
                LabelId = MilkFatLabelId,
                Name = "do 1%"
            },
            new()
            {
                Id = MilkFatValue1To2,
                LabelId = MilkFatLabelId,
                Name = "1% do 2%"
            },
            new()
            {
                Id = MilkFatValue2To3,
                LabelId = MilkFatLabelId,
                Name = "2% do 3%"
            },
            new()
            {
                Id = MilkFatValueMoreThan3,
                LabelId = MilkFatLabelId,
                Name = "iznad 3%"
            },

            new()
            {
                Id = MilkProducerValueDukat,
                LabelId = MilkProducerLabelId,
                Name = "Dukat"
            },
            new()
            {
                Id = MilkProducerValueZbregov,
                LabelId = MilkProducerLabelId,
                Name = "z bregov"
            },
            new()
            {
                Id = MilkProducerValueAlpsko,
                LabelId = MilkProducerLabelId,
                Name = "Alpsko"
            },
            new()
            {
                Id = MilkProducerValueBelje,
                LabelId = MilkProducerLabelId,
                Name = "Belje"
            }
        };
    }
}