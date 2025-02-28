using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Shipping.API.Data.Repository
{
    public class ShippingRepository : IShippingRepository
    {

        private readonly ShippingDbContext _dbContext;

        public ShippingRepository(ShippingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Entities.Shipping entity)
        {
            await _dbContext.Shippings.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Entities.Shipping> entities)
        {
            await _dbContext.Shippings.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Shippings.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Entities.Shipping>> FindAsync(Expression<Func<Entities.Shipping, bool>> predicate)
        {
            return await _dbContext.Shippings.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Shipping>> GetAllAsync()
        {
            return await _dbContext.Shippings.ToListAsync();
        }

        public async Task<Entities.Shipping> GetByIdAsync(int id)
        {
            return await _dbContext.Shippings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                return false;
            }

            _dbContext.Shippings.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task RemoveRangeAsync(IEnumerable<Entities.Shipping> entities)
        {
            _dbContext.Shippings.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Entities.Shipping entity)
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
