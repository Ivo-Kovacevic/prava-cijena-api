using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class ProductStoreConfiguration : IEntityTypeConfiguration<ProductStore>
{
    public void Configure(EntityTypeBuilder<ProductStore> builder)
    {
        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Product>(ps => ps.Product)
            .WithMany(p => p.ProductStores)
            .HasForeignKey(ps => ps.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Store>(ps => ps.Store)
            .WithMany(s => s.StoreProducts)
            .HasForeignKey(ps => ps.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(ps => ps.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(ps => ps.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}