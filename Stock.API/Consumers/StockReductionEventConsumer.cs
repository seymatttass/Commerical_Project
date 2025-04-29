using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events.OrderCreatedEvent;
using Stock.API.Data;

namespace Stock.API.Consumers
{
    public class StockReductionEventConsumer : IConsumer<StockReductionEvent>
    {
        private readonly StockDbContext _stockDbContext;

        public StockReductionEventConsumer(StockDbContext stockDbContext)
        {
            _stockDbContext = stockDbContext;
        }

        public async Task Consume(ConsumeContext<StockReductionEvent> context)
        {
            try
            {
                foreach (var orderItem in context.Message.OrderItems)
                {
                    var stock = await _stockDbContext.Stocks
                        .FirstOrDefaultAsync(s => s.ProductId == orderItem.ProductId);

                    if (stock != null)
                    {
                        _stockDbContext.Entry(stock).Property("Count").CurrentValue =
                            (int)_stockDbContext.Entry(stock).Property("Count").CurrentValue - orderItem.Count;
                    }
                    else
                    {
                        Console.WriteLine($"Ürün ID: {orderItem.ProductId} için stok bulunamadı.");
                    }
                }

                await _stockDbContext.SaveChangesAsync();

                Console.WriteLine($"Sipariş {context.Message.OrderId} için stok miktarları başarıyla azaltıldı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sipariş {context.Message.OrderId} için stok azaltma sırasında hata: {ex.Message}");

                await context.Publish(new OrderFailEvent(context.Message.CorrelationId)
                {
                    OrderId = context.Message.OrderId,
                    Message = $"Stok azaltma sırasında hata: {ex.Message}"
                });
            }
        }
    }
}