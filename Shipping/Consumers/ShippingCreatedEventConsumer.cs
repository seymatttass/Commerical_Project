using MassTransit;
using Shared.Events.ShippingEvents;
using System;
using System.Threading.Tasks;

namespace Shipping.API.Consumers
{
    public class ShippingCreatedEventConsumer : IConsumer<ShippingCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ShippingCreatedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ShippingCreatedEvent> context)
        {
       
        }
    }
}