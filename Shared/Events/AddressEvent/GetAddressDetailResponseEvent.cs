using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.AddressEvent
{
    public class GetAddressDetailResponseEvent :CorrelatedBy<Guid>
    {
        public GetAddressDetailResponseEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }

        public int AddressId { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string AddressText { get; set; }

    }
}
