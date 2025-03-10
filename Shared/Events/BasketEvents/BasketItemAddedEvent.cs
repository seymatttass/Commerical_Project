using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.BasketEvents
{
    //Basket.API ürünü sepete ekler ve BasketItemAddedEvent ile işlemin tamamlandığını bildirir 
    public class BasketItemAddedEvent
    {
        public Guid CorrelationId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductName { get; set; }
    }
}
