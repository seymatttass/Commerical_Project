using MassTransit;
using Order.API.Data;
using Shared.Events.OrderCreatedEvent;
using System;

namespace Order.API.Consumers
{
    public class OrderCompletedEventConsumer(OrderDbContext orderDbContext) : IConsumer<OrderCompletedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            Order.API.Data.Entities.Orderss order = await orderDbContext.Orderss.FindAsync(context.Message.OrderId);
            if (order != null) //orderıd ye karşılık sipariş varsa if e girer.
            {
                order.OrderStatus = Data.Enums.OrdeStatus.Completed;
                await orderDbContext.SaveChangesAsync();
            }


        }
    }
}
