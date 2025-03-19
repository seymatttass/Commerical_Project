using MassTransit;
using Shared.Events.ShippingEvents;
using System.Threading.Tasks;

namespace Order.API.Consumers
{
    public class ShippingCompletedEventConsumer : IConsumer<ShippingCompletedEvent>
    {
        public ShippingCompletedEventConsumer()
        {
        }

        public async Task Consume(ConsumeContext<ShippingCompletedEvent> context)
        {
          
        }
    }
}