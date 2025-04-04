using Address.API.Data.Entities;
using System.Linq.Expressions;

namespace Address.API.Data.Repository
{
    public interface IAddressRepository
    {
        Task<Addres> GetByIdAsync(int id);

        Task<IEnumerable<Addres>> GetAllAsync();

        Task<IEnumerable<Addres>> FindAsync(Expression<Func<Addres, bool>> predicate);

        Task AddAsync(Addres entity);
        Task AddRangeAsync(IEnumerable<Addres> entities);

        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Addres> entities);

        Task<bool> UpdateAsync(Addres entity);

        Task<bool> ExistsAsync(int id);
    }
}
