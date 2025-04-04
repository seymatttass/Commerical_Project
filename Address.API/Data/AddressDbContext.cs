using Address.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Address.API.Data
{
    public class AddressDbContext : DbContext
    {
        public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options)
        {
        }

        public DbSet<Addres> Addres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Addres>(entity =>
            {
            });
        }
    }
}


