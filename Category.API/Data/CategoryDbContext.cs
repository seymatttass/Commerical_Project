using Microsoft.EntityFrameworkCore;

namespace Category.API.Data
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions options) : base(options)
        {
        }
        //1
        public DbSet<Entities.Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Category>(entity =>
            {
                //entity.ToTable("Addres");
                //entity.HasKey(e => e.Id);
                //entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
                //entity.Property(e => e.CategoryDescription).HasMaxLength(500);
            });
        }

    }
}
