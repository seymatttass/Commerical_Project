using Basket.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Basket.API.Data.Repository.Basket
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketDbContext _dbContext;

        public BasketRepository(BasketDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Baskett entity)
        {
            await _dbContext.Baskets.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Baskett> entities)
        {
            await _dbContext.Baskets.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Baskets.AnyAsync(x => x.ID == id);
        }

        public async Task<IEnumerable<Baskett>> FindAsync(Expression<Func<Baskett, bool>> predicate)
        {
            return await _dbContext.Baskets.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Baskett>> GetAllAsync()
        {
            return await _dbContext.Baskets.ToListAsync();
        }

        public async Task<Baskett> GetByIdAsync(int id)
        {
            return await _dbContext.Baskets.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var basket = await GetByIdAsync(id);
            if (basket == null)
            {
                return false;
            }

            _dbContext.Baskets.Remove(basket);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task RemoveRangeAsync(IEnumerable<Baskett> entities)
        {
            _dbContext.Baskets.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Baskett entity)
        {
            var update = await GetByIdAsync(entity.ID);
            if (update == null)
                return false;

            _dbContext.Entry(update).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
