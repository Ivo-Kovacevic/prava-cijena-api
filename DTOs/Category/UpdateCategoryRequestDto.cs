namespace api.DTOs.Category;

public class UpdateCategoryRequestDto
{
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
}