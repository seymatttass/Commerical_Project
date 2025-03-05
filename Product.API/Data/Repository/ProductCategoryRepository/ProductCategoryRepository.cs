using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Product.API.Data.Repository.CategoryProductRepository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {

        private readonly ProductDbContext _dbContext;

        public ProductCategoryRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Entities.ProductCategory entity)
        {
            await _dbContext.ProductCategories.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Entities.ProductCategory> entities)
        {
            await _dbContext.ProductCategories.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.ProductCategories.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Entities.ProductCategory>> FindAsync(Expression<Func<Entities.ProductCategory, bool>> predicate)
        {
            return await _dbContext.ProductCategories.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Entities.ProductCategory>> GetAllAsync()
        {
            return await _dbContext.ProductCategories.ToListAsync();
        }

        public async Task<Entities.ProductCategory> GetByIdAsync(int id)
        {
            return await _dbContext.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                return false;
            }

            _dbContext.ProductCategories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task RemoveRangeAsync(IEnumerable<Entities.ProductCategory> entities)
        {
            _dbContext.ProductCategories.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Entities.ProductCategory entity)
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
