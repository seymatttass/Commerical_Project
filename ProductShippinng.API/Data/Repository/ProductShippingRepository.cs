using ProductShippinng.API.Data.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace ProductShippinng.API.Data.Repository
{
    public class ProductShippingRepository : IProductShippingRepository
    {

        private readonly ProductShippingDbContext _dbContext;

        public ProductShippingRepository(ProductShippingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ProductShipping entity)
        {
            await _dbContext.ProductShippings.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<ProductShipping> entities)
        {
            await _dbContext.ProductShippings.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.ProductShippings.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ProductShipping>> FindAsync(Expression<Func<ProductShipping, bool>> predicate)
        {
            return await _dbContext.ProductShippings.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<ProductShipping>> GetAllAsync()
        {
            return await _dbContext.ProductShippings.ToListAsync();
        }

        public async Task<ProductShipping> GetByIdAsync(int id)
        {
            return await _dbContext.ProductShippings.FirstOrDefaultAsync(x => x.Id == id);
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


        public async Task RemoveRangeAsync(IEnumerable<ProductShipping> entities)
        {
            _dbContext.ProductShippings.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(ProductShipping entity)
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

