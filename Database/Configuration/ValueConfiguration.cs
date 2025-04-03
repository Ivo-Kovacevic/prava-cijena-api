using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Database.Seeders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class ValueConfiguration : IEntityTypeConfiguration<Value>
{
    public void Configure(EntityTypeBuilder<Value> builder)
    {
        builder.Property(v => v.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Label>(v => v.Label)
            .WithMany(l => l.Values)
            .HasForeignKey(v => v.LabelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(v => v.Slug);

        builder.Property(v => v.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(v => v.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(ValueSeedingData.InitialValues());
    }
}