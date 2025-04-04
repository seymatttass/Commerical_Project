using Microsoft.EntityFrameworkCore;

namespace Shipping.API.Data
{
    public class ShippingDbContext : DbContext
    {
        public ShippingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.Shipping> Shippings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Shipping>(entity =>
            {
               
            });
        }


    }
}
