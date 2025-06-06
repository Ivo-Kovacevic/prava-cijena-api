using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Database.Seeders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.Property(s => s.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasIndex(s => s.Slug)
            .IsUnique();

        builder.Property(s => s.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(StoreSeedingData.InitialStores());
    }
}