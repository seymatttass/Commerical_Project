using Microsoft.EntityFrameworkCore;

namespace Product.API.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.Product> Products { get; set; }
        public DbSet<Entities.Category> Categories { get; set; }
        public DbSet<Entities.ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Product>(entity =>
            {

            });

            modelBuilder.Entity<Entities.Category>(entity =>
            {

            });

            modelBuilder.Entity<Entities.ProductCategory>(entity =>
            {

            });
        }

    }
}
