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
        public State BasketItemCompleted { get; private set; }
        public State PaymentStarted { get; private set; }
        public State PaymentCompleted { get; private set; }
        public State PaymentFailed { get; private set; }
        public State OrderCreated { get; private set; }
        public State OrderCompleted { get; private set; }
        public State OrderFailed { get; private set; }
        public State UserReceived { get; private set; }

        // Saga tarafından dinlenecek Event'ler
        public Event<ProductAddedToBasketRequestEvent> ProductAddedToBasketRequestEvent { get; private set; }
        public Event<StockReservedEvent> StockReservedEvent { get; private set; }
        public Event<StockNotReservedEvent> StockNotReservedEvent { get; private set; }
        public Event<BasketCompletedEvent> BasketItemCompletedEvent { get; private set; }
        public Event<PaymentStartedEvent> PaymentStartedEvent { get; private set; }
        public Event<PaymentCompletedEvent> PaymentCompletedEvent { get; private set; }
        public Event<PaymentFailedEvent> PaymentFailedEvent { get; private set; }
        public Event<OrderCreatedEvent> OrderCreatedEvent { get; private set; }
        public Event<OrderFailEvent> OrderFailEvent { get; private set; }
        public Event<OrderCompletedEvent> OrderCompletedEvent { get; private set; }
        public Event<GetAddressDetailResponseEvent> GetAddressDetailResponseEvent { get; private set; }
        public Event<GetUserDetailResponseEvent> GetUserDetailResponseEvent { get; private set; }

        public OrderStateMachine()
        {
            // Hangi property'nin saga durumunu tutacağını bildiriyoruz.
            InstanceState(x => x.CurrentState);


            Event(() => ProductAddedToBasketRequestEvent,  //orderstartedevenrt gelirse tetikleyici old. anlar.
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

            Event(() => GetAddressDetailResponseEvent,
               orderStateInstance => orderStateInstance.CorrelateById(@event =>
               @event.Message.CorrelationId));

            Event(() => GetUserDetailResponseEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));


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
                    // Stok kontrolü yapmıyoruz, direkt sepet tamamlama mesajı gönderiyoruz
                    .Send(new Uri($"queue:{RabbitMQSettings.Basket_BasketCompletedEventQueue}"),
                          context => new BasketCompletedEvent(context.Instance.CorrelationId)
                          {
                              ProductId = context.Instance.ProductId,
                              UserId = context.Instance.UserId,
                              Count = context.Instance.Count,
                              TotalPrice = context.Instance.TotalPrice
                          })
            );

            // ** ProductAdded ** durumunda sepet tamamlandı olayını bekliyoruz
            During(ProductAdded,
                When(BasketItemCompletedEvent)
                    .Then(context =>
                    {
                        // Sepet tamamlandı, şimdi stok kontrolü yapabiliriz
                    })
                    .TransitionTo(BasketItemCompleted)
                    // Sepet tamamlandıktan sonra stok kontrolü için Stock servisine mesaj gönderelim
                    .Send(new Uri($"queue:{RabbitMQSettings.Stock_CheckStockQueue}"),
                          context => new StockCheckedEvent(context.Instance.CorrelationId)
                          {
                              ProductId = context.Instance.ProductId,
                              Count = context.Instance.Count
                          })
            );

            // ** BasketItemCompleted ** durumunda stok sonucunu bekliyoruz
            During(BasketItemCompleted,
                When(StockReservedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsStockReserved = true;
                    })
                    .TransitionTo(StockReserved)
                    // Stok rezerve edildikten sonra ödeme başlatma mesajı gönderilebilir
                    .Send(new Uri($"queue:{RabbitMQSettings.Payment_PaymentStartedQueue}"),
                          context => new PaymentStartedEvent(context.Instance.CorrelationId)
                          {
                              TotalPrice = context.Instance.TotalPrice
                          }),

                When(StockNotReservedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsStockNotReserved = true;
                    })
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
                When(PaymentStartedEvent)
                    .Then(context =>
                    {
                        // Ödeme başlatıldıysa PaymentStarted durumuna geçiyoruz
                    })
                    .TransitionTo(PaymentStarted)
            );

            // ** StockNotReserved ** durumunda süreç tamamlanıyor
            During(StockNotReserved,
                Ignore(OrderFailEvent),
                // İsterseniz burada başarısız sipariş durumuna geçebilirsiniz
                When(OrderFailEvent)
                    .TransitionTo(OrderFailed)
                    .Finalize()
            );

            // ** PaymentStarted ** durumunda ödeme sonucunu bekliyoruz
            During(PaymentStarted,
                When(PaymentCompletedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsPaymentCompleted = true;
                    })
                    .TransitionTo(PaymentCompleted)
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderCreatedQueue}"),
                          context => new OrderCreatedEvent(context.Data.CorrelationId)
                          {
                              OrderId = context.Instance.OrderId,
                          }),

                When(PaymentFailedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsPaymentFailed = true;
                    })
                    .TransitionTo(PaymentFailed)
                    // Ödeme başarısız oldu, stok geri alınmalı
                    .Send(new Uri($"queue:{RabbitMQSettings.Stock_RollbackMessageQueue}"),
                          context => new StockRollBackMessage(context.Data.CorrelationId)
                          {
                              BasketItems = new List<BasketItemMessage>
                              {
                                  new BasketItemMessage
                                  {
                                      ProductId = context.Instance.ProductId,
                                      Count = context.Instance.Count
                                  }
                              }
                          })
                    // Siparişin başarısız olduğunu Order servisine bildir
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderFailedQueue}"),
                          context => new OrderFailEvent(context.Data.CorrelationId)
                          {
                              OrderId = context.Instance.OrderId,
                              Message = context.Data.Message
                          })
            );

            // ** PaymentCompleted ** durumunda sipariş oluşturuldu olayını bekliyoruz
            During(PaymentCompleted,
                When(OrderCreatedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsOrderCreated = true;
                        context.Instance.OrderId = context.Data.OrderId;
                    })
                    .TransitionTo(OrderCreated)
            );

            // ** PaymentFailed ** durumunda sipariş başarısız olayını bekliyoruz
            During(PaymentFailed,
                When(OrderFailEvent)
                    .Then(context =>
                    {
                        context.Instance.IsOrderFailed = true;
                    })
                    .TransitionTo(OrderFailed)
                    .Finalize()
            );

            // ** OrderCreated ** durumunda sipariş tamamlandı olayını bekliyoruz
            During(OrderCreated,
                When(OrderCompletedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsOrderCompleted = true;
                        context.Instance.AddressId = context.Data.AddressId;
                    })
                    .TransitionTo(OrderCompleted)
                    // Kullanıcı bilgilerini almak için mesaj gönder
                    .Send(new Uri($"queue:{RabbitMQSettings.GetUserDetailRequestEvent}"),
                          context => new GetUserDetailRequestEvent(context.Data.CorrelationId)
                          {
                              UserId = context.Instance.UserId
                          })
            );

            // ** OrderCompleted ** durumunda kullanıcı bilgilerini bekliyoruz
            During(OrderCompleted,
                When(GetUserDetailResponseEvent)
                    .Then(context =>
                    {
                        // Kullanıcı bilgilerini Saga instance'ına kaydet
                        context.Instance.Username = context.Data.Username;
                        context.Instance.Name = context.Data.Name;
                        context.Instance.Surname = context.Data.Surname;
                        context.Instance.Email = context.Data.Email;
                        context.Instance.TelNo = context.Data.TelNo;
                        context.Instance.Birthdate = context.Data.Birthdate;
                    })
                    .TransitionTo(UserReceived)
                    // Adres bilgilerini almak için mesaj gönder
                    .Send(new Uri($"queue:{RabbitMQSettings.GetAddressDetailRequestEvent}"),
                          context => new GetAddressDetailRequestEvent(context.Data.CorrelationId)
                          {
                              AddressId = context.Instance.AddressId
                          })
            );

            // ** UserReceived ** durumunda adres bilgilerini bekliyoruz
            During(UserReceived,
                When(GetAddressDetailResponseEvent)
                    .Then(context =>
                    {
                        // Adres bilgilerini Saga instance'ına kaydet
                        context.Instance.Title = context.Data.Title;
                        context.Instance.City = context.Data.City;
                        context.Instance.Country = context.Data.Country;
                        context.Instance.District = context.Data.District;
                        context.Instance.AddressText = context.Data.AddressText;
                        context.Instance.PostalCode = context.Data.PostalCode;
                    })
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
    }
}