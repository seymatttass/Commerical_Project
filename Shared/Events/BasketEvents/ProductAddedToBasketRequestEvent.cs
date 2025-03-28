﻿using MassTransit;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.BasketEvents
{
    //Kullanıcı sepete ürün eklemek istediğinde ProductAddedToBasketRequestEvent tetiklenir
    public class ProductAddedToBasketRequestEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public ProductAddedToBasketRequestEvent()
        {
            CorrelationId = Guid.NewGuid();
        }

        public int ProductId { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public List<BasketItemMessage> BasketItemMessages { get; set; }

        
    }
}