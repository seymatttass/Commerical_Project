using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.OrderCreatedEvent
{
    public class OrderFailEvent : CorrelatedBy<Guid>
    {
        public OrderFailEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public int OrderId { get; set; }
        public string Message { get; set; }
        public Guid CorrelationId { get; }
    }
}
