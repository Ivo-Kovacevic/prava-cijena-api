using System.ComponentModel.DataAnnotations.Schema;

namespace PravaCijena.Api.Models;

public class SavedStore : BaseEntity
{
    public required string UserId { get; set; }
    public required Guid StoreLocationId { get; set; }

    [NotMapped] public User User { get; set; }
    [NotMapped] public StoreLocation StoreLocation { get; set; }
}