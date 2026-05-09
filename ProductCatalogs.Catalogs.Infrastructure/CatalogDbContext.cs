using Microsoft.EntityFrameworkCore;
using ProductCatalogs.Catalogs.Domain.Models;

namespace ProductCatalogs.Catalogs.Infrastructure
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Catalog> Catalogs => Set<Catalog>();

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Catalog>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name);
            });
        }
    }
}
