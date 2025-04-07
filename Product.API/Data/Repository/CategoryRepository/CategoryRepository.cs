using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Product.API.Data.Repository.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductDbContext _dbContext;

        public CategoryRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Entities.Category entity)
        {
            await _dbContext.Categories.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Entities.Category> entities)
        {
            await _dbContext.Categories.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Categories.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Entities.Category>> FindAsync(Expression<Func<Entities.Category, bool>> predicate)
        {
            return await _dbContext.Categories.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Entities.Category> GetByIdAsync(int id)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category == null)
            {
                // Eğer veritabanında böyle bir kategori yoksa
                return false;
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }



        public async Task RemoveRangeAsync(IEnumerable<Entities.Category> entities)
        {
            _dbContext.Categories.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Entities.Category entity)
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
