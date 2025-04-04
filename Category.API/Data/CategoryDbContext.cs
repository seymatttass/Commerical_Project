using Microsoft.EntityFrameworkCore;

namespace Category.API.Data
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Category>(entity =>
            {
            });
        }

    }
}
