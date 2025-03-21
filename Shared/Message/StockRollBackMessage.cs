﻿using MassTransit;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Message
{
        public class StockRollBackMessage : CorrelatedBy<Guid>
        {
            public StockRollBackMessage(Guid correlationId)
            {
                CorrelationId = correlationId;
            }
            public List<OrderItemMessage> OrderItems { get; set; }

            public Guid CorrelationId { get; }
        }
}

