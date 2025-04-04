using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Database.Configuration;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<ProductStore> ProductStores { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Value> Values { get; set; }
    public DbSet<ProductValue> ProductValues { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new LabelConfiguration());
        builder.ApplyConfiguration(new ValueConfiguration());
        builder.ApplyConfiguration(new StoreConfiguration());
        builder.ApplyConfiguration(new ProductStoreConfiguration());
        builder.ApplyConfiguration(new PriceConfiguration());
        builder.ApplyConfiguration(new ProductValueConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
            if (entry.State == EntityState.Modified)
            {
                // entry.Property("CreatedAt").IsModified = false;
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }

        return await base.SaveChangesAsync(cancellationToken);
    }
}