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
                //entity.ToTable("Addres");
                //entity.HasKey(e => e.Id);
                //entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
                //entity.Property(e => e.CategoryDescription).HasMaxLength(500);
            });
        }
    }
}


