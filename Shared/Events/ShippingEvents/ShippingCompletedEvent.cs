using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.ShippingEvents
{
    public class ShippingCompletedEvent : CorrelatedBy<Guid>
    {
        public ShippingCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; }
        public int ShippingId { get; set; }
        public int CargoCompanyName { get; set; }
    }
}
