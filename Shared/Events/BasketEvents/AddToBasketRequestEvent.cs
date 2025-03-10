using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.BasketEvents
{
    //Stok yeterliyse SagaStateMachine AddToBasketRequestEvent'i yayınlar
    public class AddToBasketRequestEvent
    {
        public Guid CorrelationId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
    }
}
    