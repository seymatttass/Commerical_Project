using Microsoft.EntityFrameworkCore;
using Users.API.Data.Entities;

namespace Users.API.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Userss>Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // İlişkileri ve konfigürasyonları burada tanımlayabilirsiniz.
        }
    }
}
