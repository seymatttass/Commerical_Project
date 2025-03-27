using MassTransit;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.StockReductionEvent
{
    public class StockReductionEvent : CorrelatedBy<Guid>
    {
        
        public StockReductionEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public List<BasketItemMessage> OrderItems { get; set; }


    }
}
