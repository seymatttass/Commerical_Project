using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.ShippingEvents
{
    public class ShippingCompletedEvent
    {
        public int ShippingId { get; set; }
        public int CargoCompanyName { get; set; }
    }
}
