using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Database.Seeders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class ProductValueConfiguration : IEntityTypeConfiguration<ProductValue>
{
    public void Configure(EntityTypeBuilder<ProductValue> builder)
    {
        builder.Property(pv => pv.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Product>(pv => pv.Product)
            .WithMany(p => p.ProductValues)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Value>(pv => pv.Value)
            .WithMany(v => v.ValueProducts)
            .HasForeignKey(pv => pv.ValueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(pv => new { pv.ProductId, OptionId = pv.ValueId });

        builder.Property(v => v.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(v => v.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(ProductValuesSeedingData.InitialProductValues());
    }
}