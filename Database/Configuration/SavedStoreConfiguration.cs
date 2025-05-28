using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database.Configuration;

public class SavedStoreConfiguration : IEntityTypeConfiguration<SavedStore>
{
    public void Configure(EntityTypeBuilder<SavedStore> builder)
    {
        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.HasOne<StoreLocation>(ss => ss.StoreLocation)
            .WithMany(sl => sl.SavedStores)
            .HasForeignKey(ss => ss.StoreLocationId);

        builder.HasOne<User>(ss => ss.User)
            .WithMany(u => u.SavedStores)
            .HasForeignKey(ss => ss.UserId);

        builder.HasIndex(c => new { c.UserId, c.StoreLocationId }).IsUnique();

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}