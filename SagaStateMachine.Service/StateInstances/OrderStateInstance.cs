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
        public int BasketId { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}







// Adres bilgileri için eklenen propertyler
//public int AddressId { get; set; }
//public string Title { get; set; }
//public string City { get; set; }
//public string Country { get; set; }
//public string District { get; set; }
//public string AddressText { get; set; }
//public string PostalCode { get; set; }


// Userss entity'sine göre user bilgileri propertyleri
//public string Username { get; set; }
//public string Name { get; set; }
//public string Surname { get; set; }
//public string Email { get; set; }
//public DateTime? Birthdate { get; set; }
//public string TelNo { get; set; }


//public bool IsShippingCreated { get; set; } = false;
//public bool IsShippingCompleted { get; set; } = false;
//public bool IsShippingFailed { get; set; } = false;

//// Shipping verileri
//public int ShippingId { get; set; }
//public int CargoCompanyName { get; set; }
//public int ShippingCharge { get; set; }
//public bool FreeShipping { get; set; }
//public int EstimatedDays { get; set; }
