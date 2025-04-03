using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Seeders;

public class LabelSeedingData : CategorySeedingData
{
    // Milk labels
    public static readonly Guid MilkTypeLabelId = new("cd54d494-4e7f-4a36-ac2b-0c5b47f1c96b");
    public static readonly Guid MilkLifeLabelId = new("2ce02301-83c2-4fb7-8397-185a4ea2e78b");
    public static readonly Guid MilkFatLabelId = new("43b33b76-69cc-4424-8608-eeb20f931476");
    public static readonly Guid MilkProducerLabelId = new("bf131788-50bf-413a-94c2-92c8b1de1acf");

    public static IEnumerable<Label> InitialLabels()
    {
        return new List<Label>
        {
            /*
             * Milk labels
             */
            new()
            {
                Id = MilkTypeLabelId,
                Name = "Vrsta mlijeka",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = MilkLifeLabelId,
                Name = "Trajnost mlijeka",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = MilkFatLabelId,
                Name = "Mliječna masnoća",
                CategoryId = MilkSubcategoryId
            },
            new()
            {
                Id = MilkProducerLabelId,
                Name = "Proizvođač",
                CategoryId = MilkSubcategoryId
            }
        };
    }
}