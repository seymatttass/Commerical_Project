﻿using MassTransit;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.PaymentEvents
{
    public class PaymentStartedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; }
        public PaymentStartedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public decimal TotalPrice { get; set; }
        public List<BasketItemMessage> BasketItemMessages { get; set; }
    }
}
