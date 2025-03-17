using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachine.Service.StateInstances
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }


        public bool IsStockReserved { get; set; }  
        public bool IsStockNotReserved { get; set; } 
        public bool IsPaymentCompleted { get; set; }  
        public bool IsPaymentFailed { get; set; } 
        public bool IsOrderCreated { get; set; } 
        public bool IsOrderFailed { get; set; }  
    }
}
