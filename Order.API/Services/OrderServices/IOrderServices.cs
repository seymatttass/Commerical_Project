using Order.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Services.OrderServices
{
    public interface IOrderServices
    {
        Task<Orderss> GetByIdAsync(int id);
        Task<IEnumerable<Orderss>> GetAllAsync();
        Task AddAsync(Orderss order);
        Task<bool> UpdateAsync(Orderss order);
        Task<bool> RemoveAsync(int id);
    }
}
