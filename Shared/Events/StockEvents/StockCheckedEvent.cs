using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.StockEvents
{
    public class StockCheckedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; }
        public StockCheckedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
