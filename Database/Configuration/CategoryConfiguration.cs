using api.Database.Seeders;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Database.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Category>(c => c.ParentCategory)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Slug)
            .IsUnique();

        builder.HasIndex(c => new { c.Name, c.Slug })
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("english");

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(CategorySeedingData.InitialCategories());
    }
}