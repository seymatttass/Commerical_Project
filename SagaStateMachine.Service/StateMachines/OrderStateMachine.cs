using MassTransit;
using SagaStateMachine.Service.StateInstances;
using Shared.Events.AddressEvent;
using Shared.Events.BasketEvents;
using Shared.Events.OrderCreatedEvent;
using Shared.Events.PaymentEvents;
using Shared.Events.StockEvents;
using Shared.Events.StockReductionEvent;
using Shared.Events.UserEvents;
using Shared.Message;
using Shared.Messages;
using Shared.Settings;

namespace SagaStateMachine.Service.StateMachines
{
    /// <summary>
    /// Bu sınıf, bir sipariş işlem sürecini yöneten Saga State Machine'i tanımlar.
    /// MassTransit kütüphanesi kullanılarak, mikroservisler arasındaki iletişimi koordine eder.
    /// </summary>
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        // *** DURUMLAR (STATES) ***
        // Saga'nın ilerleyebileceği tüm durumları tanımlıyoruz.
        public State ProductAdded { get; private set; }
        public State StockReserved { get; private set; }
        public State StockNotReserved { get; private set; }
        public State StockReduced { get; private set; }
        public State PaymentStarted { get; private set; }
        public State PaymentCompleted { get; private set; }
        public State PaymentFailed { get; private set; }
        public State OrderCreated { get; private set; }

        /// <summary>Sipariş tamamlandı durumu</summary>
        public State OrderCompleted { get; private set; }

        /// <summary>Sipariş başarısız oldu durumu</summary>
        public State OrderFailed { get; private set; }
        //public State UserReceived { get; private set; }

        // Saga'nın dinleyeceği tüm olayları tanımlıyoruz. Bu olaylar, farklı mikroservislerden
        // gelen mesajları temsil eder ve Saga'nın durumunu değiştirmesine neden olur.

        public Event<ProductAddedToBasketRequestEvent> ProductAddedToBasketRequestEvent { get; private set; }
        public Event<StockReservedEvent> StockReservedEvent { get; private set; }
        public Event<StockNotReservedEvent> StockNotReservedEvent { get; private set; }
        public Event<PaymentStartedEvent> PaymentStartedEvent { get; private set; }
        public Event<PaymentCompletedEvent> PaymentCompletedEvent { get; private set; }
        public Event<PaymentFailedEvent> PaymentFailedEvent { get; private set; }
        public Event<StockReductionEvent> StockReductionEvent { get; private set; }
        public Event<OrderFailEvent> OrderFailEvent { get; private set; }
        public Event<OrderCompletedEvent> OrderCompletedEvent { get; private set; }

        //public Event<GetAddressDetailResponseEvent> GetAddressDetailResponseEvent { get; private set; }
        //public Event<GetUserDetailResponseEvent> GetUserDetailResponseEvent { get; private set; }                

        /// <summary>
        /// OrderStateMachine constructor'ı
        /// Saga'nın tüm davranışını, durum geçişlerini ve olay korelasyonlarını burada tanımlıyoruz.
        /// </summary>
        public OrderStateMachine()
        {
            // OrderStateInstance sınıfının CurrentState property'sinin, 
            // Saga'nın mevcut durumunu tutacağını belirtiyoruz.
            InstanceState(x => x.CurrentState);

            // *** OLAY KORELASYONLARI ***
            // Her olayın hangi Saga instance'ı ile ilişkilendirileceğini tanımlıyoruz.
            // CorrelationId kullanarak, gelen olayların doğru Saga instance'ına yönlendirilmesini sağlıyoruz.

            // ProductAddedToBasketRequestEvent için korelasyon tanımı
            // İlk olay olduğu için, yeni bir Guid oluşturarak yeni bir Saga instance'ı başlatıyoruz.
            Event(() => ProductAddedToBasketRequestEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.CorrelationId)
                .SelectId(e => Guid.NewGuid()));

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

            Event(() => StockReductionEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            Event(() => OrderFailEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));

            Event(() => OrderCompletedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event =>
                @event.Message.CorrelationId));


            // İlk gelen olay: ProductAddedToBasketRequestEvent
            // Bu olay geldiğinde, yeni bir Saga instance'ı oluşturulur ve ProductAdded durumuna geçilir.
            Initially(
                When(ProductAddedToBasketRequestEvent)
                    .Then(context =>
                    {
                        // Saga instance'ına temel bilgileri kaydet
                        context.Instance.UserId = context.Data.UserId;
                        context.Instance.CreatedDate = DateTime.UtcNow;
                        context.Instance.TotalPrice = context.Data.Price * context.Data.Count;
                        context.Instance.ProductId = context.Data.ProductId;
                        context.Instance.Count = context.Data.Count;
                        context.Instance.Price = context.Data.Price;
                    })
                    .TransitionTo(ProductAdded)
                    // Stok kontrolü için mesaj gönder
                    .Send(new Uri($"queue:{RabbitMQSettings.Stock_CheckStockQueue}"),
                      context => new StockCheckedEvent(context.Instance.CorrelationId)
                      {
                          ProductId = context.Instance.ProductId,
                          Count = context.Instance.Count
                      }));

            During(ProductAdded,
                When(StockReservedEvent)
                    .TransitionTo(StockReserved)
                    .Send(new Uri($"queue:{RabbitMQSettings.Payment_PaymentStartedQueue}"),
                          context => new PaymentStartedEvent(context.Instance.CorrelationId)
                          {
                              TotalPrice = context.Instance.TotalPrice
                          }),

                When(StockNotReservedEvent)
                    .TransitionTo(StockNotReserved)
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderFailedQueue}"),
                          context => new OrderFailEvent(context.Data.CorrelationId)
                          {
                              OrderId = context.Instance.OrderId,
                              Message = "Stok yetersiz."
                          })
            );

          
            During(StockReserved,
                When(PaymentCompletedEvent)
                    .TransitionTo(PaymentCompleted)
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderCompletedQueue}"),
                        context => new OrderCompletedEvent(context.Instance.CorrelationId)
                        {
                            OrderId = context.Instance.OrderId
                        }),
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

            During(PaymentCompleted,
                When(OrderCompletedEvent)
                    .TransitionTo(OrderCompleted)
                    .Send(new Uri($"queue:{RabbitMQSettings.Stock_ReductionQueue}"),
                        context => new StockReductionEvent(context.Instance.CorrelationId)
                        {
                            OrderId = context.Instance.OrderId,
                            ProductId = context.Instance.ProductId,
                            Count = context.Instance.Count
                        })
            );


            During(OrderCompleted,
                When(StockReductionEvent)
                    .TransitionTo(StockReduced)
                    // Saga'yı sonlandır
                    .Finalize()
            );

            // Saga sonlandırıldığında completed statüsüne geçmesini sağlar
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

