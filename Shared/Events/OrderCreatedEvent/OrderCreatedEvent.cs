using MassTransit;
using Shared.Messages;
using System;
using System.Collections.Generic;

namespace Shared.Events.OrderCreatedEvent
{
    public class CreateOrderCommand : CorrelatedBy<Guid>
    {
        public CreateOrderCommand(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
        public int UserId { get; set; }
        public int BasketId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BasketItemMessage> BasketItemMessages { get; set; }
    }
}
