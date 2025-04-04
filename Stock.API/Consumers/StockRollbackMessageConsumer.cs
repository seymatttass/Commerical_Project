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
            foreach (var basketItem in context.Message.BasketItemMessages) 
            {
                var stock = await stockDbContext.Stocks
                     .FirstOrDefaultAsync(x => x.ProductId == basketItem.ProductId);

                if (stock != null)
                {
                    int currentCount = stock.Count;
                    stock.Count = currentCount + basketItem.Count;
                }
            }
            await stockDbContext.SaveChangesAsync();
        }
    }
}