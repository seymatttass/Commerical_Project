using System.Linq.Expressions;

namespace Stock.API.Data.Repository
{
    public interface IStockRepository
    {

        Task<Entities.Stock> GetByIdAsync(int id);

        Task<IEnumerable<Entities.Stock>> GetAllAsync();

        Task<IEnumerable<Entities.Stock>> FindAsync(Expression<Func<Entities.Stock, bool>> predicate);

        Task AddAsync(Entities.Stock entity);
        Task AddRangeAsync(IEnumerable<Entities.Stock> entities);

        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Stock> entities);

        Task<bool> UpdateAsync(Entities.Stock entity);

        Task<bool> ExistsAsync(int id);
    }
}
