namespace api.Dto.Store;

public class StoreDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string StoreUrl { get; set; }
    public required string ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}