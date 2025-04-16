using MassTransit;
using Order.API.Data;
using Shared.Events.OrderCreatedEvent;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Order.API.Consumers
{
    public class OrderFailedEventConsumer : IConsumer<OrderFailEvent>
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly ILogger<OrderFailedEventConsumer> _logger;

        public OrderFailedEventConsumer(OrderDbContext orderDbContext, ILogger<OrderFailedEventConsumer> logger)
        {
            _orderDbContext = orderDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderFailEvent> context)
        {
            _logger.LogInformation("OrderFailEvent alındı. SiparişId: {OrderId}", context.Message.OrderId);

            var order = await _orderDbContext.Orderss.FindAsync(context.Message.OrderId);

            if (order != null)
            {
                order.OrderStatus = Data.Enums.OrdeStatus.Failed;
                await _orderDbContext.SaveChangesAsync();
                _logger.LogInformation("Sipariş durumu 'Failed' olarak güncellendi. SiparişId: {OrderId}", context.Message.OrderId);
            }
            else
            {
                _logger.LogWarning("Sipariş bulunamadı. SiparişId: {OrderId}", context.Message.OrderId);
            }
        }
    }
}
