using Order.API.Data.Entities;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Data.Repository.OrderItem
{
    public interface IOrderItemRepository
    {
        Task<OrderItemss> GetByIdAsync(int id);
        Task<IEnumerable<OrderItemss>> GetAllAsync();
        Task<IEnumerable<OrderItemss>> FindAsync(Expression<Func<OrderItemss, bool>> predicate);
        Task AddAsync(OrderItemss entity);
        Task AddRangeAsync(IEnumerable<OrderItemss> entities);
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<OrderItemss> entities);
        Task<bool> UpdateAsync(OrderItemss entity);
        Task<bool> ExistsAsync(int id);
    }
}
