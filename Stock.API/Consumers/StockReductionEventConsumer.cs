using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events.OrderCreatedEvent;
using Shared.Events.StockReductionEvent;
using Stock.API.Data;
using Stock.API.Data.Entities;

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
            // Bu consumer sipariş tamamlandıktan sonra sadece stok düşürme işlemini yapar
            try
            {
                // Ürünlerin stok miktarını azalt
                foreach (var orderItem in context.Message.OrderItems)
                {
                    var stock = await _stockDbContext.Stocks
                        .FirstOrDefaultAsync(s => s.ProductId == orderItem.ProductId);

                    if (stock != null)
                    {
                        // Stok miktarını, Stock entity'sinin property'si ile azalt
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
                // Hata durumunda loglama yap
                Console.WriteLine($"Sipariş {context.Message.OrderId} için stok azaltma sırasında hata: {ex.Message}");

                // Hata olduğunda OrderFailEvent gönder
                await context.Publish(new OrderFailEvent(context.Message.CorrelationId)
                {
                    OrderId = context.Message.OrderId,
                    Message = $"Stok azaltma sırasında hata: {ex.Message}"
                });
            }
        }
    }
}