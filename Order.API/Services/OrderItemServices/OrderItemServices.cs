using Order.API.Data.Repository.OrderItem;
using Order.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Services.OrderItemServices
{
    public class OrderItemServices : IOrderItemServices
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemServices(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<OrderItemss> GetByIdAsync(int id)
        {
            return await _orderItemRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<OrderItemss>> GetAllAsync()
        {
            return await _orderItemRepository.GetAllAsync();
        }

        public async Task AddAsync(OrderItemss orderItem)
        {
            await _orderItemRepository.AddAsync(orderItem);
        }

        public async Task<bool> UpdateAsync(OrderItemss orderItem)
        {
            return await _orderItemRepository.UpdateAsync(orderItem);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            return await _orderItemRepository.RemoveAsync(id);
        }
    }
}
