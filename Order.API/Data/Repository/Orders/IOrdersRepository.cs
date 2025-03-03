using Order.API.Data.Entities;
using System.Linq.Expressions;

namespace Order.API.Data.Repository.Orders
{
    public interface IOrdersRepository
    {
        Task<Orderss> GetByIdAsync(int id);

        Task<IEnumerable<Orderss>> GetAllAsync();

        Task<IEnumerable<Orderss>> FindAsync(Expression<Func<Orderss, bool>> predicate);

        Task AddAsync(Orderss entity);

        Task AddRangeAsync(IEnumerable<Orderss> entities);

        Task<bool> RemoveAsync(int id);

        Task RemoveRangeAsync(IEnumerable<Orderss> entities);

        Task<bool> UpdateAsync(Orderss entity);

        Task<bool> ExistsAsync(int id);
    }
}
