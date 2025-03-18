using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.ShippingEvents
{
    public class ShippingCreatedEvent
    {
        public int CargoCompanyName { get; set; }
        public bool Active { get; set; }
        public int Shipcharge { get; set; }
        public bool free { get; set; }
        public int EstimatedDays { get; set; }
    }
}
