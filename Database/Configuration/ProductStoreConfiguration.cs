using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Database.Configuration;

public class ProductStoreConfiguration : IEntityTypeConfiguration<ProductStore>
{
    public void Configure(EntityTypeBuilder<ProductStore> builder)
    {
        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Product>(p => p.Product)
            .WithMany(p => p.ProductStores)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Store>(p => p.Store)
            .WithMany(s => s.StoreProducts)
            .HasForeignKey(p => p.StoreId);

        builder.Property(ps => ps.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(ps => ps.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}