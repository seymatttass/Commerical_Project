using Logging.Shared.Models;
using Logging.Shared.Models.Logging.Shared.Models;
using MassTransit;

namespace Logging.Shared.Services
{
    public class LogPublisher : ILogPublisher
    {
        public readonly IPublishEndpoint _publishEndpoint;

        public LogPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishLogAsync(LogMessage message)
        {
            await _publishEndpoint.Publish(message);
        }
    }
}
