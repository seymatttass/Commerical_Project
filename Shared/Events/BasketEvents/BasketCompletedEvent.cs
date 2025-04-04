using MassTransit;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.BasketEvents
{
    public class BasketCompletedEvent : CorrelatedBy<Guid>
    {
        public BasketCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }



    }
}
