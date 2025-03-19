using MassTransit;
using Shared.Events.ShippingEvents;
using System.Threading.Tasks;

namespace Order.API.Consumers
{
    public class ShippingFailedEventConsumer : IConsumer<ShippingFailedEvent>
    {
        public ShippingFailedEventConsumer()
        {
        }

        public async Task Consume(ConsumeContext<ShippingFailedEvent> context)
        {
          
        }
    }
}