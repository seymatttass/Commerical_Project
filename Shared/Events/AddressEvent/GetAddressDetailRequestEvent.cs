using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.AddressEvent
{
    public class GetAddressDetailRequestEvent : CorrelatedBy<Guid>
    {
        public GetAddressDetailRequestEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public int AddressId { get; set; }
        public Guid CorrelationId { get; set; }




    }
}
