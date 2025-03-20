using api.Models;

namespace api.Dto.Store;

public class StoreDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string StoreUrl { get; set; }
    public required string ImageUrl { get; set; }
}