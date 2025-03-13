using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.BasketEvents
{
    //Stok yeterliyse SagaStateMachine AddToBasketRequestEvent'i yayınlar
    public class AddToBasketRequestEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; }
        public AddToBasketRequestEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
    }
}
    