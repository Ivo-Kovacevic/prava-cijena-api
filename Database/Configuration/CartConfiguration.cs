using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Product>(c => c.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(c => c.ProductId);

        builder.HasOne<User>(c => c.User)
            .WithMany(u => u.CartItems)
            .HasForeignKey(c => c.UserId);

        builder.HasIndex(c => new { c.UserId, c.ProductId }).IsUnique();

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}