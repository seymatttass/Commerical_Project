using MassTransit;
using Shared.Events.PaymentEvents;
using Shared.Settings;
using Microsoft.Extensions.Logging;

namespace Payment.API.Consumers
{
    public class PaymentStartedEventConsumer : IConsumer<PaymentStartedEvent>
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILogger<PaymentStartedEventConsumer> _logger;

        public PaymentStartedEventConsumer(ISendEndpointProvider sendEndpointProvider, ILogger<PaymentStartedEventConsumer> logger)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentStartedEvent> context)
        {
            _logger.LogInformation("PaymentStartedEvent alındı. KorelasyonId: {CorrelationId}", context.Message.CorrelationId);

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));

            if (true) // Buraya ödeme başarılı olup olmadığını kontrol eden gerçek mantığı ekleyebilirsiniz.
            {
                PaymentCompletedEvent paymentCompletedEvent = new(context.Message.CorrelationId);
                await sendEndpoint.Send(paymentCompletedEvent);
                _logger.LogInformation("PaymentCompletedEvent başarıyla gönderildi. KorelasyonId: {CorrelationId}", context.Message.CorrelationId);
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new(context.Message.CorrelationId)
                {
                    Message = "Yetersiz bakiye...",
                };

                await sendEndpoint.Send(paymentFailedEvent);
                _logger.LogWarning("PaymentFailedEvent gönderildi. KorelasyonId: {CorrelationId}, Mesaj: Yetersiz bakiye...", context.Message.CorrelationId);
            }
        }
    }
}
