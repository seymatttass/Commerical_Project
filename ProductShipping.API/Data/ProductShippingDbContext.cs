using Microsoft.EntityFrameworkCore;

namespace ProductShipping.API.Data
{
    public class ProductShippingDbContext : DbContext
    {
        public ProductShippingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ProductShipping> ProductShippings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductShipping>(entity =>
            {
                //entity.ToTable("Addres");
                //entity.HasKey(e => e.Id);
                //entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
                //entity.Property(e => e.CategoryDescription).HasMaxLength(500);
            });
        }

    }
}
