using MassTransit;

namespace Shared.Events.CategoryEvents
{


    public class CategoryUpdatedEvent : CorrelatedBy<Guid>
    {
        public CategoryUpdatedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }


    }
}
