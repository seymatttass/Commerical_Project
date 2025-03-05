using Order.API.Data.Repository.Orders;
using Order.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Services.OrderServices
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrderServices(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<Orderss> GetByIdAsync(int id)
        {
            return await _ordersRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Orderss>> GetAllAsync()
        {
            return await _ordersRepository.GetAllAsync();
        }

        public async Task AddAsync(Orderss order)
        {
            await _ordersRepository.AddAsync(order);
        }

        public async Task<bool> UpdateAsync(Orderss order)
        {
            return await _ordersRepository.UpdateAsync(order);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            return await _ordersRepository.RemoveAsync(id);
        }
    }
}
