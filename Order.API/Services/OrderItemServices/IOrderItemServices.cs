using Order.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Services.OrderItemServices
{
    public interface IOrderItemServices
    {
        Task<OrderItemss> GetByIdAsync(int id);
        Task<IEnumerable<OrderItemss>> GetAllAsync();
        Task AddAsync(OrderItemss orderItem);
        Task<bool> UpdateAsync(OrderItemss orderItem);
        Task<bool> RemoveAsync(int id);
    }
}
