using Microsoft.EntityFrameworkCore;

namespace ProductShipping.Data
{
    public class ProductShippingDbContext : DbContext
    {
        public ProductShippingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.ProductShipping> ProductShippings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.ProductShipping>(entity =>
            {
                //entity.ToTable("Addres");
                //entity.HasKey(e => e.Id);
                //entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
                //entity.Property(e => e.CategoryDescription).HasMaxLength(500);
            });
        }

    }
}
