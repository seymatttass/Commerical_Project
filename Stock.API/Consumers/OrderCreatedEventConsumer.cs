using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
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
            // tüm siparişlere,ürünlere bakıp stok sağlanıyor ise stok ile ilgili gerekli güncellemeleri sağlıyoruz
            // stok sağlanmıyorsa stoknotreserved i publish ederiz yayınlarız.
            List<bool> stockResults = new();

            //siparişte her bir talep edilen ürüne karşılık db de bu ürünü bulup
            //bunun counutunu değeerlendiricez.siparişteki talep edilenden fazlamı karşılanıyor mu 
            foreach (var orderItem in context.Message.OrderItems)
            {
                var stockExists = await stockDbContext.Stocks
                    .AnyAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count);
                stockResults.Add(stockExists);
            }

            //stokresults içini kontrol edicez eğer tüm değerler true ise ona göre stoğu güncelleyeceğiz ,
            // biri bile false gelirse stoknotreserved i publis edicez ve bu evente subscribe olan ilgili serviselrde ona göre davranışlarını yönetecekler.

            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));

            if (stockResults.TrueForAll(s => s.Equals(true)))
            {
                foreach (var orderItem in context.Message.OrderItems)
                {
                    //db deki ilgili verileri elde edelim.
                    var stock = await stockDbContext.Stocks
                        .FirstOrDefaultAsync(s => s.ProductId == orderItem.ProductId);
                      stock.Count -= orderItem.Count;
                }

                await stockDbContext.SaveChangesAsync();

                StockReservedEvent stockReservedEvent = new(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems,

                };
                await sendEndpoint.Send(stockReservedEvent);
            }
            //stokda problem varsa:
            else
            {
                StockNotReservedEvent stockNotReservedEvent = new(context.Message.CorrelationId)
                {
                    Message = "Stok yetersiz..."
                };
                await sendEndpoint.Send(stockNotReservedEvent);
            }

        }
    }
}
