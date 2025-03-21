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

            // BasketItemCompletedEvent'i burada yayınlamamalısın
            // Bu event StockReservedEvent'ten sonra State Machine tarafından tetiklenmeli
        }
    }
}
