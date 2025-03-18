using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.CategoryEvents
{
    public class CategoryDeletedEvent : CorrelatedBy<Guid>
    {
        public CategoryDeletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }
        public int CategoryId { get; set; }

    }
}
