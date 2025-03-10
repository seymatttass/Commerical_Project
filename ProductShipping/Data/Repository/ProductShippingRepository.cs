using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;



namespace ProductShipping.Data.Repository
{
    public class ProductShippingRepository : IProductShippingRepository
    {

        private readonly ProductShippingDbContext _dbContext;

        public ProductShippingRepository(ProductShippingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Entities.ProductShipping entity)
        {
            await _dbContext.ProductShippings.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Entities.ProductShipping> entities)
        {
            await _dbContext.ProductShippings.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.ProductShippings.AnyAsync(x => x.ProductId == id);
        }

        public async Task<IEnumerable<Entities.ProductShipping>> FindAsync(Expression<Func<Entities.ProductShipping, bool>> predicate)
        {
            return await _dbContext.ProductShippings.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Entities.ProductShipping>> GetAllAsync()
        {
            return await _dbContext.ProductShippings.ToListAsync();
        }

        public async Task<Entities.ProductShipping> GetByIdAsync(int id)
        {
            return await _dbContext.ProductShippings.FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                return false;
            }

            _dbContext.ProductShippings.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task RemoveRangeAsync(IEnumerable<Entities.ProductShipping> entities)
        {
            _dbContext.ProductShippings.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Entities.ProductShipping entity)
        {
            var update = await GetByIdAsync(entity.ProductId);
            if (update == null)
                return false;

            _dbContext.Entry(update).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return true;

        }

    }
}
