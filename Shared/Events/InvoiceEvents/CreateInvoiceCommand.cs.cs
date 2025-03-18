// Shared/Events/InvoiceEvents/CreateInvoiceCommand.cs
using MassTransit;
using System;

namespace Shared.Events.InvoiceEvents
{
    public class CreateInvoiceCommand : CorrelatedBy<Guid>
    {
        public CreateInvoiceCommand(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Cargoficheno { get; set; }
    }
}