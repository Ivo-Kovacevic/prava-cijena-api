using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Database;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasOne<Category>(c => c.ParentCategory)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(c => c.Slug)
            .IsUnique();
        
        builder.HasIndex(c => c.Slug)
            .IsUnique();

        builder.Property(c => c.CreatedAt)
            .HasDefaultValue(DateTime.Now);
            
        builder.Property(c => c.UpdatedAt)
            .HasDefaultValue(DateTime.Now);
    }
}