using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database.Configuration;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<StoreLocation> StoreLocations { get; set; }
    public DbSet<StoreCategory> StoreCategories { get; set; }
    public DbSet<ProductStore> ProductStores { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Value> Values { get; set; }
    public DbSet<ProductValue> ProductValues { get; set; }

    public DbSet<Cart> Cart { get; set; }
    public DbSet<SavedProduct> SavedProducts { get; set; }
    public DbSet<SavedStore> SavedStores { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new LabelConfiguration());
        builder.ApplyConfiguration(new ValueConfiguration());
        builder.ApplyConfiguration(new StoreConfiguration());
        builder.ApplyConfiguration(new StoreLocationConfiguration());
        builder.ApplyConfiguration(new StoreCategoryConfiguration());
        builder.ApplyConfiguration(new ProductStoreConfiguration());
        builder.ApplyConfiguration(new PriceConfiguration());
        builder.ApplyConfiguration(new ProductValueConfiguration());

        builder.ApplyConfiguration(new CartConfiguration());
        builder.ApplyConfiguration(new SavedProductConfiguration());
        builder.ApplyConfiguration(new SavedStoreConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
            if (entry.State == EntityState.Modified)
            {
                var updatedAtProperty = entry.Metadata.FindProperty("UpdatedAt");
                if (updatedAtProperty != null)
                {
                    // You might also want to check if the property type is compatible, e.g., DateTime
                    if (updatedAtProperty.ClrType == typeof(DateTime) || updatedAtProperty.ClrType == typeof(DateTime?))
                    {
                        entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                    }
                }
            }

        return await base.SaveChangesAsync(cancellationToken);
    }
}