using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class ProductStoreLinkConfiguration : IEntityTypeConfiguration<ProductStoreLink>
{
    public void Configure(EntityTypeBuilder<ProductStoreLink> builder)
    {
        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Product>(psl => psl.Product)
            .WithMany(p => p.ProductStoreLinks)
            .HasForeignKey(c => c.ProductId);

        builder.HasOne<Store>(psl => psl.Store)
            .WithMany(s => s.StoreProductLinks)
            .HasForeignKey(c => c.StoreId);

        builder.HasIndex(c => new { c.StoreId, c.ProductId }).IsUnique();

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}