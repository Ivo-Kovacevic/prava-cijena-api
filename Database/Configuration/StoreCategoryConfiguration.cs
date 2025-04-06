using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Database.Seeders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class StoreCategoryConfiguration : IEntityTypeConfiguration<StoreCategory>
{
    public void Configure(EntityTypeBuilder<StoreCategory> builder)
    {
        builder.Property(sc => sc.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Store>(sc => sc.Store)
            .WithMany(s => s.Categories)
            .HasForeignKey(sc => sc.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sc => sc.ParentStoreCategory)
            .WithMany(sc => sc.Subcategories)
            .HasForeignKey(sc => sc.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(s => s.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(StoreCategorySeedingData.InitialStoreCategories());
    }
}