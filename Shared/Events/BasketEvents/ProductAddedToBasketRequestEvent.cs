﻿using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.BasketEvents
{
    //Kullanıcı sepete ürün eklemek istediğinde ProductAddedToBasketRequestEvent tetiklenir
    public class ProductAddedToBasketRequestEvent:CorrelatedBy<Guid>
    {
        public ProductAddedToBasketRequestEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }    
        public decimal Price { get; set; }
    }
}