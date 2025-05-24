namespace PravaCijena.Api.Models;

public class Pagination
{
    public required int Page { get; set; }
    public required int Limit { get; set; }
    public required int TotalProducts { get; set; }
    public required int TotalPages { get; set; }
}