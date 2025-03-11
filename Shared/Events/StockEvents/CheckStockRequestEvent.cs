using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.StockEvents
{
    //ProductAddedToBasketRequestEvent SagaStateMachine bu eventi alır ve CheckStockRequestEvent'i yayınlar
    // ProductAddedToBasketRequestEvent -> SagaStateMachine bu eventi alır -> Stock.API stok kontrolü yapar
    public class CheckStockRequestEvent
    {
        public Guid CorrelationId { get; set; }  // Saga süreci için gerekli ID
        public int ProductId { get; set; } // Hangi ürünün kontrol edildiği
        public int RequestedCount { get; set; } // Kullanıcının almak istediği miktar
    }
}
