using Microsoft.EntityFrameworkCore;
using Order.API.Data.Entities;

namespace Order.API.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<OrderItemss> OrderItems { get; set; }
        public DbSet<Orderss> Orderss { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItemss>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<Orderss>(entity =>
            {
                entity.HasKey(e => e.ID);
            });
        }
    }
}
