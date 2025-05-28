using System.ComponentModel.DataAnnotations.Schema;

namespace PravaCijena.Api.Models;

public class Cart : BaseEntity
{
    public required string UserId { get; set; }
    public required Guid ProductId { get; set; }

    [NotMapped] public User User { get; set; }
    [NotMapped] public Product Product { get; set; }
}