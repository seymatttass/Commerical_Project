using Microsoft.EntityFrameworkCore;
using Payment.API.Data.Entities;

namespace Payment.API.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Paymentt> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // İlişkileri ve konfigürasyonları burada tanımlayabilirsiniz.
        }
    }
}
