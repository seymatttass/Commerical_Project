using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events.OrderCreatedEvent;
using Shared.Message;
using Stock.API.Data;

namespace Stock.API.Consumers
{
    public class StockRollbackMessageConsumer(StockDbContext stockDbContext) : IConsumer<StockRollBackMessage>
    {
        public async Task Consume(ConsumeContext<StockRollBackMessage> context)
        {
            // Payment API'de bir ödeme sorunu olursa, Stock API'de yapılmış olan stok işlemlerinin geri alımını gerçekleştiriyoruz
            // Compensable transaction
            foreach (var basketItem in context.Message.BasketItems) // OrderItems yerine BasketItems kullanın
            {
                var stock = await stockDbContext.Stocks
                     .FirstOrDefaultAsync(x => x.ProductId == basketItem.ProductId);

                if (stock != null)
                {
                    // Count özelliğini artırın (stock entity'nizdeki Count özelliği)
                    // Özelliği bir değişkene atayıp işlem yaparak çakışmayı önleyelim
                    int currentCount = stock.Count;
                    stock.Count = currentCount + basketItem.Count;
                }
            }
            await stockDbContext.SaveChangesAsync();
        }
    }
}