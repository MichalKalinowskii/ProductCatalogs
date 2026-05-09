using Microsoft.EntityFrameworkCore;
using ProductCatalogs.Products.Domain.Models;

namespace ProductCatalogs.Products.Infrastructure
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.CatalogId);
                e.Property(x => x.Name);
                e.Property(x => x.Description);
                e.Property(x => x.CreatedDate);
                e.Property(x => x.UpdatedDate);
                e.Property(x => x.Price);
                e.Property(x => x.Code);
            });
        }
    }
}