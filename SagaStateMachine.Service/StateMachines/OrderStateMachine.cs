using MassTransit;
using SagaStateMachine.Service.StateInstances;
using Shared.Events.AddressEvent;
using Shared.Events.BasketEvents;
using Shared.Events.OrderCreatedEvent;
using Shared.Events.PaymentEvents;
using Shared.Events.StockEvents;
using Shared.Events.UserEvents;
using Shared.Message;
using Shared.Messages;
using Shared.Settings;

namespace SagaStateMachine.Service.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        // Saga boyunca kullanılacak durumlar (State)
        public State ProductAdded { get; private set; }
        public State StockReserved { get; private set; }
        public State StockNotReserved { get; private set; }
        public State PaymentStarted { get; private set; }
        public State PaymentCompleted { get; private set; }
        public State PaymentFailed { get; private set; }
        public State OrderCreated { get; private set; }
        public State OrderCompleted { get; private set; }
        public State OrderFailed { get; private set; }
        //public State UserReceived { get; private set; }

        // Saga tarafından dinlenecek Event'ler
        public Event<ProductAddedToBasketRequestEvent> ProductAddedToBasketRequestEvent { get; private set; }
        public Event<StockReservedEvent> StockReservedEvent { get; private set; }
        public Event<StockNotReservedEvent> StockNotReservedEvent { get; private set; }
        public Event<PaymentStartedEvent> PaymentStartedEvent { get; private set; }
        public Event<PaymentCompletedEvent> PaymentCompletedEvent { get; private set; }
        public Event<PaymentFailedEvent> PaymentFailedEvent { get; private set; }
        public Event<OrderCreatedEvent> OrderCreatedEvent { get; private set; }
        public Event<OrderFailEvent> OrderFailEvent { get; private set; }
        public Event<OrderCompletedEvent> OrderCompletedEvent { get; private set; }
        //public Event<GetAddressDetailResponseEvent> GetAddressDetailResponseEvent { get; private set; }
        //public Event<GetUserDetailResponseEvent> GetUserDetailResponseEvent { get; private set; }

        public OrderStateMachine()
        {
            // Hangi property'nin saga durumunu tutacağını bildiriyoruz.
            InstanceState(x => x.CurrentState);
            Event(() => ProductAddedToBasketRequestEvent,  //orderstartedevent gelirse tetikleyici old. anlar.
                orderStateInstance => orderStateInstance.CorrelateById<int>(database =>
                database.OrderId, @event => @event.Message.BasketId) //eşit mi?
                //varsa yeni bir corelationıd oluşturmayacağız.
                //yoksa oluşturucaz.
                .SelectId(e => Guid.NewGuid())); //yeni correlationıd.

            Event(() => StockReservedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            Event(() => StockNotReservedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            Event(() => PaymentStartedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            Event(() => PaymentCompletedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            Event(() => PaymentFailedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            Event(() => OrderCreatedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            Event(() => OrderFailEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            Event(() => OrderCompletedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            //Event(() => GetAddressDetailResponseEvent,
            //   orderStateInstance => orderStateInstance.CorrelateById(@event =>
            //   @event.Message.CorrelationId));

            //Event(() => GetUserDetailResponseEvent,
            //    orderStateInstance => orderStateInstance.CorrelateById(@event =>
            //    @event.Message.CorrelationId));


            // 2) Durum geçişleri (Initially, During blokları):

            // **Initially**:
            // İlk gelen event: ProductAddedToBasketRequestEvent
            Initially(
                When(ProductAddedToBasketRequestEvent)
                    .Then(context =>
                    {
                        // Saga instance'ına temel bilgileri kaydet
                        context.Instance.UserId = context.Data.UserId;
                        context.Instance.CreatedDate = DateTime.Now;
                        context.Instance.TotalPrice = context.Data.Price * context.Data.Count;
                        context.Instance.ProductId = context.Data.ProductId;
                        context.Instance.Count = context.Data.Count;
                        context.Instance.Price = context.Data.Price;
                    })
                    .TransitionTo(ProductAdded)
                       .Send(new Uri($"queue:{RabbitMQSettings.Stock_CheckStockQueue}"),
                      context => new StockCheckedEvent(context.Instance.CorrelationId)
                      {
                          ProductId = context.Instance.ProductId,
                          Count = context.Instance.Count
                      }));

            During(ProductAdded,
                When(StockReservedEvent)
                    .TransitionTo(StockReserved)
                    // Stok rezerve edildikten sonra ödeme başlatma mesajı gönderilebilir
                    .Send(new Uri($"queue:{RabbitMQSettings.Payment_PaymentStartedQueue}"),
                          context => new PaymentStartedEvent(context.Instance.CorrelationId)
                          {
                              TotalPrice = context.Instance.TotalPrice
                          }),

                When(StockNotReservedEvent)
                    .TransitionTo(StockNotReserved)
                    // Sipariş başarısız bilgisini OrderFailEvent olarak Order servisine gönder
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderFailedQueue}"),
                          context => new OrderFailEvent(context.Data.CorrelationId)
                          {
                              OrderId = context.Instance.OrderId,
                              Message = "Stok yetersiz."
                          })
            );

            // ** StockReserved ** durumunda ödeme başlatma olayını bekliyoruz
            During(StockReserved,
                When(PaymentCompletedEvent)
                    .TransitionTo(PaymentCompleted)
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderCompletedQueue}"),
                      context => new OrderCompletedEvent(context.Instance.CorrelationId)
                      {
                          OrderId = context.Instance.OrderId,
                      })
                     .Finalize(),
                 When(PaymentFailedEvent)
                   .TransitionTo(PaymentFailed)
                   .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderFailedQueue}"),
                      context => new OrderFailEvent(context.Instance.CorrelationId)
                      {
                          OrderId = context.Instance.OrderId,
                          Message = context.Data.Message
                      })
                      .Send(new Uri($"queue:{RabbitMQSettings.Stock_RollbackMessageQueue}"),
                       context => new StockRollBackMessage(context.Instance.CorrelationId)
                       {
                           BasketItemMessages = context.Data.BasketItemMessages
                       }));

            SetCompletedWhenFinalized();
        }
    }
}



































//// ** OrderCompleted ** durumunda kullanıcı bilgilerini bekliyoruz
//During(OrderCompleted,
//    When(GetUserDetailResponseEvent)
//        .Then(context =>
//        {
//            // Kullanıcı bilgilerini Saga instance'ına kaydet
//            context.Instance.Username = context.Data.Username;
//            context.Instance.Name = context.Data.Name;
//            context.Instance.Surname = context.Data.Surname;
//            context.Instance.Email = context.Data.Email;
//            context.Instance.TelNo = context.Data.TelNo;
//            context.Instance.Birthdate = context.Data.Birthdate;
//        })
//        .TransitionTo(UserReceived)
//        // Adres bilgilerini almak için mesaj gönder
//        .Send(new Uri($"queue:{RabbitMQSettings.GetAddressDetailRequestEvent}"),
//              context => new GetAddressDetailRequestEvent(context.Data.CorrelationId)
//              {
//                  AddressId = context.Instance.AddressId
//              })
//);

//// ** UserReceived ** durumunda adres bilgilerini bekliyoruz
//During(UserReceived,
//    When(GetAddressDetailResponseEvent)
//        .Then(context =>
//        {
//            // Adres bilgilerini Saga instance'ına kaydet
//            context.Instance.Title = context.Data.Title;
//            context.Instance.City = context.Data.City;
//            context.Instance.Country = context.Data.Country;
//            context.Instance.District = context.Data.District;
//            context.Instance.AddressText = context.Data.AddressText;
//            context.Instance.PostalCode = context.Data.PostalCode;
//        })
//        .Finalize()
//);

