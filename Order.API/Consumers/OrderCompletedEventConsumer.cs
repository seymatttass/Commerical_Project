using MassTransit;
using Order.API.Data;
using Shared.Events.OrderCreatedEvent;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Order.API.Consumers
{
    public class OrderCompletedEventConsumer : IConsumer<OrderCompletedEvent>
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly ILogger<OrderCompletedEventConsumer> _logger;

        public OrderCompletedEventConsumer(OrderDbContext orderDbContext, ILogger<OrderCompletedEventConsumer> logger)
        {
            _orderDbContext = orderDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            _logger.LogInformation("OrderCompletedEvent alındı. SiparişId: {OrderId}, KorelasyonId: {CorrelationId}", context.Message.OrderId, context.Message.CorrelationId);

            try
            {
                var order = await _orderDbContext.Orderss.FindAsync(context.Message.OrderId);

                if (order != null)
                {
                    _logger.LogInformation("Sipariş bulundu. SiparişId: {OrderId}", context.Message.OrderId);
                    order.OrderStatus = Data.Enums.OrdeStatus.Completed;
                    await _orderDbContext.SaveChangesAsync();
                    _logger.LogInformation("Sipariş durumu başarıyla güncellendi. SiparişId: {OrderId}, Yeni Durum: {OrderStatus}", context.Message.OrderId, order.OrderStatus);
                }
                else
                {
                    _logger.LogWarning("Sipariş bulunamadı. SiparişId: {OrderId}", context.Message.OrderId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş durumu güncellenirken bir hata oluştu. SiparişId: {OrderId}", context.Message.OrderId);
                throw;
            }
        }
    }
}
