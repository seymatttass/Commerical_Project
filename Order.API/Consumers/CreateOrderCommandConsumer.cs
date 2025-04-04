using MassTransit;
using Order.API.Data;
using Order.API.Data.Entities;
using Shared.Events.OrderCreatedEvent;
using System.Threading.Tasks;

namespace Order.API.Consumers
{
    public class CreateOrderCommandConsumer : IConsumer<CreateOrderCommand>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandConsumer(OrderDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<CreateOrderCommand> context)
        {
            var order = new Orderss
            {
                UserId = context.Message.UserId,
                BasketId = context.Message.BasketId,
                TotalPrice = context.Message.TotalPrice,
                CretaedDate = DateTime.UtcNow,
                OrderStatus = Data.Enums.OrdeStatus.Suspend,
            };

            await _context.Orderss.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in context.Message.BasketItemMessages)
            {
                await _context.OrderItems.AddAsync(new OrderItemss
                {
                    OrderId = order.ID,
                    ProductId = item.ProductId,
                    Count = item.Count,
                    TotalPrice = item.Price * item.Count
                });
            }

            await _context.SaveChangesAsync();

            await context.Publish(new OrderCompletedEvent(context.Message.CorrelationId)
            {
                OrderId = order.ID,
            });
        }
    }
}
