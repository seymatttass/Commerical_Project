using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.OrderCreatedEvent
{
    public class OrderCompletedEvent
    {
        public OrderCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
        public int AddressId { get; set; }

    }
}
