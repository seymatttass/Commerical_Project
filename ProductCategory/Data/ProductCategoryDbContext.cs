using Microsoft.EntityFrameworkCore;

namespace ProductCategory.Data
{
    public class ProductCategoryDbContext : DbContext
    {
        public ProductCategoryDbContext(DbContextOptions options) : base(options)
        {

        }


        public DbSet<Entities.ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.ProductCategory>(entity =>
            {
                //entity.ToTable("Addres");
                //entity.HasKey(e => e.Id);
                //entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
                //entity.Property(e => e.CategoryDescription).HasMaxLength(500);
            });
        }
    }
}
