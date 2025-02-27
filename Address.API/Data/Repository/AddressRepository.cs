using Address.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Address.API.Data.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AddressDbContext _dbContext;

        public AddressRepository(AddressDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Addres entity)
        {
            await _dbContext.Addres.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Addres> entities)
        {
            await _dbContext.Addres.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Addres.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Addres>> FindAsync(Expression<Func<Addres, bool>> predicate)
        {
            return await _dbContext.Addres.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Addres>> GetAllAsync()
        {
            return await _dbContext.Addres.ToListAsync();
        }

        public async Task<Addres> GetByIdAsync(int id)
        {
            return await _dbContext.Addres.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                return false;
            }

            _dbContext.Addres.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task RemoveRangeAsync(IEnumerable<Addres> entities)
        {
            _dbContext.Addres.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Addres entity)
        {
            var update = await GetByIdAsync(entity.Id);
            if (update == null)
                return false;

            _dbContext.Entry(update).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return true;

        }
    }
}
