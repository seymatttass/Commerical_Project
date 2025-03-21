﻿using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.StockEvents
{
    //Stock.API, stok durumunu kontrol eder ve sonucu StockCheckedResponseEvent olarak döner
    // Stock.API stok kontrolünü yaptıktan sonra sonucu döner
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
