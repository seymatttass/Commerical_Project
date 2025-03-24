using System.Linq.Expressions;
using Basket.API.Data.Entities;
namespace Basket.API.Data.Repository.Basket
{
    public interface IBasketRepository
    {
        Task<Baskett> GetByIdAsync(int id);
        Task<IEnumerable<Baskett>> GetAllAsync();
        Task<IEnumerable<Baskett>> FindAsync(Expression<Func<Baskett, bool>> predicate);
        Task AddAsync(Baskett entity);
        Task AddRangeAsync(IEnumerable<Baskett> entities);
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Baskett> entities);
        Task<bool> UpdateAsync(Baskett entity);
        Task<bool> ExistsAsync(int id);
        Task<Baskett> GetByUserIdAsync(int userId);
    }
}