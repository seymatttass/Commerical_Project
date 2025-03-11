using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.OrderCreatedEvent
{
    public class OrderCompletedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; }
        public OrderCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public int OrderId { get; set; }
    }
}
