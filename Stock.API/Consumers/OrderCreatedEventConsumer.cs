using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events.OrderCreatedEvent;
using Shared.Events.StockEvents;
using Shared.Settings;
using Stock.API.Data;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer(StockDbContext stockDbContext, ISendEndpointProvider sendEndpointProvider) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            // Bu consumer sadece stok arttırma/azaltma işlemleri için kullanılacak
            // Stok kontrol işlemi başka consumer'da yapılacak

            try
            {
                // Ürünlerin stok miktarını azalt
                foreach (var orderItem in context.Message.OrderItems)
                {
                    var stock = await stockDbContext.Stocks
                        .FirstOrDefaultAsync(s => s.ProductId == orderItem.ProductId);

                    if (stock != null)
                    {
                        stock.Count -= orderItem.Count;
                    }
                }

                await stockDbContext.SaveChangesAsync();

                // Stok başarıyla azaltıldı, ilgili event'i gönder
                var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));

                StockReservedEvent stockReservedEvent = new(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems,
                };

                await sendEndpoint.Send(stockReservedEvent);
            }
            catch (Exception ex)
            {
                // Stok azaltma sırasında bir hata oluştu
                var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));

                StockNotReservedEvent stockNotReservedEvent = new(context.Message.CorrelationId)
                {
                    Message = $"Stok azaltma işlemi sırasında hata: {ex.Message}"
                };

                await sendEndpoint.Send(stockNotReservedEvent);
            }
        }
    }
}