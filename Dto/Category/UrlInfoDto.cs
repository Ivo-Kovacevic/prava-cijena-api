namespace PravaCijena.Api.Dto.Category;

public class UrlInfoDto
{
    public required string Url { get; set; }
    public Guid? EquivalentCategoryId { get; set; }
}