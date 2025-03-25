using Basket.API.Data;
using MassTransit;
using Shared.Events.BasketEvents;
using Shared.Events.OrderCreatedEvent;
using StackExchange.Redis;
using System;

namespace Basket.API.Consumers
{

    public class BasketCompletedEventConsumer(BasketDbContext basketDbContext) : IConsumer<BasketCompletedEvent>
    {
        public async Task Consume(ConsumeContext<BasketCompletedEvent> context)
        {
            Basket.API.Data.Entities.Baskett basket = await basketDbContext.Baskets.FindAsync(context.Message.BasketId);
            if (basket != null) //basketıd ye karşılık sipariş varsa if e girer.
            {
                await basketDbContext.SaveChangesAsync();
            }


        }
    }
}


