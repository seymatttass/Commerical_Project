using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events.StockEvents;
using Shared.Messages;
using Shared.Settings;
using Stock.API.Data;

namespace Stock.API.Consumers
{
    public class StockCheckedEventConsumer(StockDbContext stockDbContext, ISendEndpointProvider sendEndpointProvider) : IConsumer<StockCheckedEvent>
    {
        public async Task Consume(ConsumeContext<StockCheckedEvent> context)
        {
            // StockCheckedEvent muhtemelen sepetteki bir ürünün stok kontrolü için kullanılıyor
            // Bu yüzden sadece ilgili ürünün stok kontrolünü yapıyoruz

            var stockExists = await stockDbContext.Stocks
                .AnyAsync(s => s.ProductId == context.Message.ProductId && s.Count >= context.Message.Count);

            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));

            if (stockExists)
            {
                // Stok yeterli, StockReservedEvent gönder
                StockReservedEvent stockReservedEvent = new(context.Message.CorrelationId)
                {
                    // OrderItems yerine basket'teki bilgileri kullanarak yaklaşımı değiştiriyoruz
                    OrderItems = new List<OrderItemMessage>
                    {
                        new OrderItemMessage
                        {
                            ProductId = context.Message.ProductId,
                            Count = context.Message.Count
                            // Diğer gerekli bilgiler varsa eklenebilir
                        }
                    }
                };
                await sendEndpoint.Send(stockReservedEvent);
            }
            else
            {
                // Stok yetersiz
                StockNotReservedEvent stockNotReservedEvent = new(context.Message.CorrelationId)
                {
                    Message = $"Ürün ID: {context.Message.ProductId} için yeterli stok bulunmuyor."
                };
                await sendEndpoint.Send(stockNotReservedEvent);
            }
        }
    }
}