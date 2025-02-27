using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Stock.API.Data.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _dbContext;

        public StockRepository(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Entities.Stock entity)
        {
            await _dbContext.Stocks.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Entities.Stock> entities)
        {
            await _dbContext.Stocks.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Entities.Stock>> FindAsync(Expression<Func<Entities.Stock, bool>> predicate)
        {
            return await _dbContext.Stocks.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Stock>> GetAllAsync()
        {
            return await _dbContext.Stocks.ToListAsync();
        }

        public async Task<Entities.Stock> GetByIdAsync(int id)
        {
            return await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                return false;
            }

            _dbContext.Stocks.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task RemoveRangeAsync(IEnumerable<Entities.Stock> entities)
        {
            _dbContext.Stocks.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Entities.Stock entity)
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
