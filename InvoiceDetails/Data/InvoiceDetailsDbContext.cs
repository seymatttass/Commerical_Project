using Microsoft.EntityFrameworkCore;

namespace InvoiceDetails.Data
{
    public class InvoiceDetailsDbContext : DbContext
    {
        public InvoiceDetailsDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Entities.InvoiceDetails> InvoiceDetailss { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.InvoiceDetails>(entity =>
            {
            });
        }
    }
}
