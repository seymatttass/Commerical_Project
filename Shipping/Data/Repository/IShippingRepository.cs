using System.Linq.Expressions;

namespace Shipping.API.Data.Repository
{
    public interface IShippingRepository
    {
        Task<Entities.Shipping> GetByIdAsync(int id);

        Task<IEnumerable<Entities.Shipping>> GetAllAsync();

        Task<IEnumerable<Entities.Shipping>> FindAsync(Expression<Func<Entities.Shipping, bool>> predicate);

        Task AddAsync(Entities.Shipping entity);
        Task AddRangeAsync(IEnumerable<Entities.Shipping> entities);

        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Shipping> entities);

        Task<bool> UpdateAsync(Entities.Shipping entity);

        Task<bool> ExistsAsync(int id);
    }
}
