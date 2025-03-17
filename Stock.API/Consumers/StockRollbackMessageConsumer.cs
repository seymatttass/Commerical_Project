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
            //payment api de bir ödeme sorunu olursa ,stokapi de yapılmış olan stok işlemlerinin geri alımını gerçekleştiriceğiz.
            //conpensable transaction


            var stockCollection = stockDbContext.Stocks;

            foreach (var orderItem in context.Message.OrderItems)
            {
                var stock = await stockDbContext.Stocks
                     .FirstOrDefaultAsync(x => x.ProductId == orderItem.ProductId);

                stock.Count += orderItem.Count;
            }
            await stockDbContext.SaveChangesAsync();
        }
    }
}
