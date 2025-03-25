using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events.OrderCreatedEvent;
using Stock.API.Data;
using Stock.API.Data.Entities;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer(StockDbContext stockDbContext) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            // Bu consumer sipariş oluşturulduktan sonra (ödeme başarılı olduktan sonra) stok düşürme işlemini yapar
            try
            {
                // Ürünlerin stok miktarını azalt
                foreach (var orderItem in context.Message.OrderItems)
                {
                    var stock = await stockDbContext.Stocks
                        .FirstOrDefaultAsync(s => s.ProductId == orderItem.ProductId);
                    
                    if (stock != null)
                    {
                        // Stok miktarını, Stock entity'sinin SetCount metodu veya property'si ile azalt
                        // Direkt değer ataması yapmak yerine EF Core track edilebilir değişiklik yap
                        stockDbContext.Entry(stock).Property("Count").CurrentValue =
                            (int)stockDbContext.Entry(stock).Property("Count").CurrentValue - orderItem.Count;
                    }
                }
                
                await stockDbContext.SaveChangesAsync();
                
                // İşlem başarılı olduğunda OrderCompletedEvent gönder
                await context.Publish(new OrderCompletedEvent(context.Message.CorrelationId)
                {
                    OrderId = context.Message.OrderId
                });
                
                Console.WriteLine($"Sipariş {context.Message.OrderId} için stok miktarları başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yap
                Console.WriteLine($"Sipariş {context.Message.OrderId} için stok güncelleme sırasında hata: {ex.Message}");
                
                // Hata olduğunda OrderFailEvent gönder
                await context.Publish(new OrderFailEvent(context.Message.CorrelationId)
                {
                    OrderId = context.Message.OrderId,
                    Message = $"Stok güncelleme sırasında hata: {ex.Message}"
                });
            }
        }
    }
}