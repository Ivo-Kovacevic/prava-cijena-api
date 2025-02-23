using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Product>()
                .HasOne<Category>(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired();

            builder.Entity<Category>()
                .HasIndex(c => c.Slug)
                .IsUnique();
            
            builder.Entity<Product>()
                .HasIndex(p => p.Slug)
                .IsUnique();
        }
    }
}