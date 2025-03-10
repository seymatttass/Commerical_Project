﻿using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ProductEvents
{
    public class ProductStartedEvent 
    {
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
