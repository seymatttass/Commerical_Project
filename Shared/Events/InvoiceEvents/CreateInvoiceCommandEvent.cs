// Shared/Events/InvoiceEvents/CreateInvoiceCommand.cs
using MassTransit;
using System;

namespace Shared.Events.InvoiceEvents
{
    public class CreateInvoiceCommandEvent : CorrelatedBy<Guid>
    {
        public CreateInvoiceCommandEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Cargoficheno { get; set; }
    }
}