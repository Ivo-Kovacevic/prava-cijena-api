using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class SavedProductConfiguration : IEntityTypeConfiguration<SavedProduct>
{
    public void Configure(EntityTypeBuilder<SavedProduct> builder)
    {
        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Product>(sp => sp.Product)
            .WithMany(p => p.SavedProducts)
            .HasForeignKey(sp => sp.ProductId);

        builder.HasOne<User>(sp => sp.User)
            .WithMany(u => u.SavedProducts)
            .HasForeignKey(sp => sp.UserId);

        builder.HasIndex(c => new { c.UserId, c.ProductId }).IsUnique();

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}