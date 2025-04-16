using Order.API.Data.Repository.Orders;
using Order.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Order.API.Services.OrderServices
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<OrderServices> _logger;

        public OrderServices(IOrdersRepository ordersRepository, ILogger<OrderServices> logger)
        {
            _ordersRepository = ordersRepository;
            _logger = logger;
        }

        public async Task<Orderss> GetByIdAsync(int id)
        {
            _logger.LogInformation("Sipariş alınıyor. SiparişId: {id}", id);
            var order = await _ordersRepository.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Sipariş bulunamadı. SiparişId: {id}", id);
            }
            else
            {
                _logger.LogInformation("Sipariş başarıyla alındı. SiparişId: {id}", id);
            }
            return order;
        }

        public async Task<IEnumerable<Orderss>> GetAllAsync()
        {
            _logger.LogInformation("Tüm siparişler alınıyor.");
            var orders = await _ordersRepository.GetAllAsync();
            _logger.LogInformation("Tüm siparişler başarıyla alındı.");
            return orders;
        }

        public async Task AddAsync(Orderss order)
        {
            _logger.LogInformation("Yeni sipariş ekleniyor. SiparişBilgisi: {OrderDetails}", order.ToString());
            await _ordersRepository.AddAsync(order);
            _logger.LogInformation("Sipariş başarıyla eklendi. SiparişId: {id}", order.ID);
        }

        public async Task<bool> UpdateAsync(Orderss order)
        {
            _logger.LogInformation("Sipariş güncelleniyor. SiparişId: {id}", order.ID);
            var result = await _ordersRepository.UpdateAsync(order);
            if (result)
            {
                _logger.LogInformation("Sipariş başarıyla güncellendi. SiparişId: {id}", order.ID);
            }
            else
            {
                _logger.LogWarning("Sipariş güncellenemedi. SiparişId: {id}", order.ID);
            }
            return result;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            _logger.LogInformation("Sipariş siliniyor. SiparişId: {id}", id);
            var result = await _ordersRepository.RemoveAsync(id);
            if (result)
            {
                _logger.LogInformation("Sipariş başarıyla silindi. SiparişId: {id}", id);
            }
            else
            {
                _logger.LogWarning("Sipariş silinemedi. SiparişId: {id}", id);
            }
            return result;
        }
    }
}
