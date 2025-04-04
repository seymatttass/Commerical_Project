using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.UserEvents
{
    public class GetUserDetailRequestEvent : CorrelatedBy<Guid>
    {

        public GetUserDetailRequestEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }

        public int UserId { get; set; }

    }

}
