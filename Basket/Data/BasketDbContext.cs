using Microsoft.EntityFrameworkCore;
using BasketEntity = Basket.API.Data.Entities.Baskett;
using BasketItemEntity = Basket.API.Data.Entities.BasketItem;

namespace Basket.API.Data
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {
        }

        public DbSet<BasketEntity> Baskets { get; set; }
        public DbSet<BasketItemEntity> BasketItems { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BasketItemEntity>();
        }
    }
}