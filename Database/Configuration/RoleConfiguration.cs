using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PravaCijena.Api.Database.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        var roles = new List<IdentityRole>
        {
            new()
            {
                Id = "aa06f708-03be-41d0-b0e1-780861fdc5d6",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Id = "29f5ab36-358e-490d-8e5a-29f4b0c904dc",
                Name = "User",
                NormalizedName = "USER"
            }
        };

        builder.HasData(roles);
    }
}