using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Database;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne<Category>(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();
            
        builder.HasIndex(p => p.Name)
            .IsUnique();
        
        builder.HasIndex(p => p.Slug)
            .IsUnique();
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValue(DateTime.Now);
            
        builder.Property(p => p.UpdatedAt)
            .HasDefaultValue(DateTime.Now);
    }
}