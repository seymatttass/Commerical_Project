using System.Linq.Expressions;

namespace Product.API.Data.Repository.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<Entities.Category> GetByIdAsync(int id);

        Task<IEnumerable<Entities.Category>> GetAllAsync();

        Task<IEnumerable<Entities.Category>> FindAsync(Expression<Func<Entities.Category, bool>> predicate);

        Task AddAsync(Entities.Category entity);
        Task AddRangeAsync(IEnumerable<Entities.Category> entities);

        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Category> entities);

        Task<bool> UpdateAsync(Entities.Category entity);

        Task<bool> ExistsAsync(int id);
    }
}
