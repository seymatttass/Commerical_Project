using MassTransit;
using Order.API.Data;
using Shared.Events.OrderCreatedEvent;

namespace Order.API.Consumers
{
    public class OrderFailedEventConsumer(OrderDbContext orderDbContext) : IConsumer<OrderFailEvent>
    {
        public async Task Consume(ConsumeContext<OrderFailEvent> context)
        {
            Order.API.Data.Entities.Orderss order = await orderDbContext.Orderss.FindAsync(context.Message.OrderId);
            if (order != null) //orderıd ye karşılık sipariş varsa if e girer.
            {
                order.OrderStatus = Data.Enums.OrdeStatus.Failed;
                await orderDbContext.SaveChangesAsync();
            }
        }
    }
}
