// Shared/Events/InvoiceEvents/InvoiceCreatedEvent.cs
using MassTransit;
using System;

namespace Shared.Events.InvoiceEvents
{
    public class InvoiceCreatedEvent : CorrelatedBy<Guid>
    {
        public InvoiceCreatedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
    }
}