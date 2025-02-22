namespace api.DTOs.Category;

public class CreateCategoryRequestDto
{
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
}