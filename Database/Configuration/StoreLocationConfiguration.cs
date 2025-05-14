using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Database.Seeders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class StoreLocationConfiguration : IEntityTypeConfiguration<StoreLocation>
{
    public void Configure(EntityTypeBuilder<StoreLocation> builder)
    {
        builder.Property(s => s.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<Store>(sl => sl.Store)
            .WithMany(p => p.StoreLocations)
            .HasForeignKey(ps => ps.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.Address)
            .IsUnique();

        builder.Property(s => s.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(StoreLocationSeedingData.InitialStoreLocations());
    }
}