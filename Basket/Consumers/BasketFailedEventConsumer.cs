using Basket.API.Data;
using MassTransit;
using Shared.Events.BasketEvents;


namespace Basket.API.Consumers
{
    public class BasketFailedEventConsumer(BasketDbContext basketDbContext) : IConsumer<BasketFailedEvent>
    {
        public async Task Consume(ConsumeContext<BasketFailedEvent> context)
        {
            Basket.API.Data.Entities.Baskett basket = await basketDbContext.Baskets.FindAsync(context.Message.BasketId);
            if (basket != null)
            {
                await basketDbContext.SaveChangesAsync();
            }
        }
    }
}
