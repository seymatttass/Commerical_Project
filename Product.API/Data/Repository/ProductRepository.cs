using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Product.API.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _dbContext;

        public ProductRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Entities.Product entity)
        {
            await _dbContext.Products.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Entities.Product> entities)
        {
            await _dbContext.Products.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Products.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Entities.Product>> FindAsync(Expression<Func<Entities.Product, bool>> predicate)
        {
            return await _dbContext.Products.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Entities.Product> GetByIdAsync(int id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var products = await GetByIdAsync(id);
            if (products != null)
            {
                return false;
            }

            _dbContext.Products.Remove(products);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task RemoveRangeAsync(IEnumerable<Entities.Product> entities)
        {
            _dbContext.Products.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Entities.Product entity)
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
