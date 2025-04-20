using Logging.Shared.Models;
using Logging.Shared.Models.Logging.Shared.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LoggingWorker.Consumers
{
    public class LoggingEventConsumer : IConsumer<LogMessage>
    {
        private readonly ILogger<LoggingEventConsumer> _logger;

        public LoggingEventConsumer(ILogger<LoggingEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<LogMessage> context)
        {
            var log = context.Message;
            _logger.LogInformation("[{Service}] {Level}: {Message}", log.Service, log.Level, log.Message);
            return Task.CompletedTask;
        }
    }
}
