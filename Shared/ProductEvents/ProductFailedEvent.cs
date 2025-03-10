using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ProductEvents
{
    public class ProductFailedEvent
    {
        public int ProductId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
