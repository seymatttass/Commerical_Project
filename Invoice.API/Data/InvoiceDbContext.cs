using Microsoft.EntityFrameworkCore;

namespace Invoice.API.Data
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Invoice>(entity =>
            {

            });
        }

    }
}
