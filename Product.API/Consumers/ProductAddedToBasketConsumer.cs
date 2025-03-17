using MassTransit;
using Shared.Events.BasketEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachine.Service.Consumers
{
    public class ProductAddedToBasketConsumer : IConsumer<ProductAddedToBasketRequestEvent>
    {
        private readonly ILogger<ProductAddedToBasketConsumer> _logger;

        public ProductAddedToBasketConsumer(ILogger<ProductAddedToBasketConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductAddedToBasketRequestEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation($"Saga tarafından ürün sepete ekleniyor: {message.Name}, Miktar: {message.Count}");

            // **Saga tamamlandıktan sonra `BasketItemCompletedEvent` tetiklenmeli**
            BasketItemCompletedEvent completedEvent = new(message.CorrelationId)
            {
                ProductId = message.ProductId,
                UserId = message.UserId,
                Count = message.Count,
                TotalPrice = message.Price * message.Count,
                Name = message.Name
            };

            await context.Publish(completedEvent);
        }
    }
}
