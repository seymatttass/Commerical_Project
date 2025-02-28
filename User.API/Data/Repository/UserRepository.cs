using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Users.API.Data.Entities;

namespace Users.API.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _dbContext;

        public UserRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Userss entity)
        {
            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Userss> entities)
        {
            await _dbContext.Users.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Users.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Userss>> FindAsync(Expression<Func<Userss, bool>> predicate)
        {
            return await _dbContext.Users.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Userss>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<Userss> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task RemoveRangeAsync(IEnumerable<Userss> entities)
        {
            _dbContext.Users.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Userss entity)
        {
            var update = await GetByIdAsync(entity.Id);
            if (update == null)
                return false;

            _dbContext.Entry(update).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Kullanıcı doğrulama işlemi
        public async Task<Userss?> AuthenticateAsync(string username, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }
    }
}
