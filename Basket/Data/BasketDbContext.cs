// Alias tanımı
using Microsoft.EntityFrameworkCore;
using BasketEntity = Basket.API.Data.Entities.Baskett;

namespace Basket.API.Data
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions options) : base(options)
        {
        }

        // Artık BasketEntity diyerek sınıfı net şekilde belirtiyoruz
        public DbSet<BasketEntity> Baskets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
