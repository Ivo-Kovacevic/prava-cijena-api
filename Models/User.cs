using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PravaCijena.Api.Models;

public class User : IdentityUser
{
    [NotMapped] public List<Cart> CartItems { get; set; } = [];
    [NotMapped] public List<SavedProduct> SavedProducts { get; set; } = [];
    [NotMapped] public List<SavedStore> SavedStores { get; set; } = [];
}