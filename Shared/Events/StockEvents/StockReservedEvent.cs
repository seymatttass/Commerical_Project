using MassTransit;
using Shared.Messages;


namespace Shared.Events.StockEvents
{
    public class StockReservedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; }
        public StockReservedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public List<BasketItemMessage> BasketItemMessages { get; set; }


    }
}

