using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public class BasketItemMessage   
        {   
            public int ProductId { get; set; }
            public int Count { get; set; }
            public decimal Price { get; set; }
            public string Name { get; set; }
    }
}