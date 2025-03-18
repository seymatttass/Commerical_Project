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




        // Saga tarafından dinlenecek Event’ler
        public Event<ProductAddedToBasketRequestEvent> ProductAddedToBasketRequestEvent { get; private set; }
        public Event<StockReservedEvent> StockReservedEvent { get; private set; }
        public Event<StockNotReservedEvent> StockNotReservedEvent { get; private set; }
        public Event<BasketItemCompletedEvent> BasketItemCompletedEvent { get; private set; }
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

            // 1) Event’lerin Correlation ayarları:
            //    Saga’yı başlatan ilk eventte .SelectId(...) ile yeni saga instance’ı yaratıyoruz.
            //    Diğer event’lerde .CorrelateById(...) ile var olan saga’yı buluyoruz.

            Event(() => ProductAddedToBasketRequestEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId)
                      .SelectId(ctx => ctx.Message.CorrelationId));

            Event(() => StockReservedEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => StockNotReservedEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => BasketItemCompletedEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => PaymentStartedEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => PaymentCompletedEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => PaymentFailedEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => OrderCreatedEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => OrderFailEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => OrderCompletedEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => GetAddressDetailResponseEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Event(() => GetUserDetailResponseEvent,
                x => x.CorrelateById(ctx => ctx.Message.CorrelationId));



            // 2) Durum geçişleri (Initially, During blokları):

            // **Initially**:
            // İlk gelen event: ProductAddedToBasketRequestEvent
            Initially(
                When(ProductAddedToBasketRequestEvent)
                    .Then(context =>
                    {
                        // Saga instance’ına bazı temel bilgileri kaydedebiliriz.
                        context.Instance.UserId = context.Data.UserId;
                        context.Instance.CreatedDate = DateTime.Now;
                        // Toplam fiyat = adet * birim fiyat
                        context.Instance.TotalPrice = context.Data.Price * context.Data.Count;
                    })
                    .TransitionTo(ProductAdded) // ProductAdded durumuna geç
                                                // Stok kontrolü için Stock servisine bir mesaj gönderelim (opsiyonel).
                    .Send(new Uri($"queue:{RabbitMQSettings.Stock_CheckStockQueue}"),
                          context => new StockCheckedEvent(context.Data.CorrelationId)
                          {
                              ProductId = context.Data.ProductId,
                              Count = context.Data.Count
                          })
            );

            // **ProductAdded** durumunda stok sonucunu bekliyoruz:
            // Stok rezerve edildiyse StockReservedEvent, edilemediyse StockNotReservedEvent
            During(ProductAdded,
                When(StockReservedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsStockReserved = true;
                        // İsteğe bağlı olarak saga instance’ı üzerinde ek setlemeler yapılabilir
                    })
                    .TransitionTo(StockReserved),

                When(StockNotReservedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsStockNotReserved = true;
                    })
                    .TransitionTo(StockNotReserved)
                    // Sipariş başarısız bilgisini OrderFailEvent olarak Order servisine gönderebilirsin.
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderFailedQueue}"),
                          context => new OrderFailEvent(context.Data.CorrelationId)
                          {
                              OrderId = context.Instance.OrderId, // Saga’da tutmuyorsanız ekleyin
                              Message = context.Data.Message
                          })
            );

            // **StockReserved** durumuna geçtikten sonra sepet işlemi (Basket) tamamlandığında
            // BasketItemCompletedEvent gelebilir.
            During(StockReserved,
                When(BasketItemCompletedEvent)
                    .Then(context =>
                    {
                        // Örneğin "BasketItemCompleted" durumuna geçip,
                        // ardından Payment servisine “PaymentStartedEvent” gönderebilirsiniz
                    })
                    .TransitionTo(BasketItemCompleted)
                    .Send(new Uri($"queue:{RabbitMQSettings.Payment_PaymentStartedQueue}"),
                          context => new PaymentStartedEvent(context.Data.CorrelationId)
                          {
                              TotalPrice = context.Instance.TotalPrice,
                              OrderItems = context.Data.OrderItems
                          })
            );

            // **StockNotReserved** durumunda isterseniz siparişi finalle de çekebilirsiniz
            // ya da ek rollback vs. logic uygulayabilirsiniz. Burada basitçe Ignore ettik.
            During(StockNotReserved,
                Ignore(OrderFailEvent)
            );

            // **BasketItemCompleted** durumunda PaymentStartedEvent’i de direkt alabiliriz.
            // (Eğer akış direkt PaymentStartedEvent ile geliyorsa.)
            During(BasketItemCompleted,
                When(PaymentStartedEvent)
                    .Then(context =>
                    {
                        // Ödeme başlatıldıysa PaymentStarted durumuna geçebiliriz
                    })
                    .TransitionTo(PaymentStarted)
            );

            // **PaymentStarted** durumunda ödeme sonucu PaymentCompletedEvent veya PaymentFailedEvent gelir
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
                              OrderItems = context.Data.OrderItems
                          }),

                When(PaymentFailedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsPaymentFailed = true;
                    })
                    .TransitionTo(PaymentFailed)
                    // Siparişin başarısız olduğunu Order servisine bildir
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderFailedQueue}"),
                          context => new OrderFailEvent(context.Data.CorrelationId)
                          {
                              OrderId = context.Instance.OrderId,
                              Message = context.Data.Message
                          })
                    // Ayrıca rezerve edilen stokları geri almak için StockRollBackMessage yollayabilirsiniz
                    .Send(new Uri($"queue:{RabbitMQSettings.Stock_RollbackMessageQueue}"),
                          context => new StockRollBackMessage(context.Data.CorrelationId)
                          {
                              OrderItems = context.Data.OrderItems
                          })
            );

            // **PaymentCompleted** durumunda OrderCreatedEvent bekleniyor (sipariş tamamı oluşmuş demek)
            // Bu event gelince OrderCreated durumuna geçip saga’yı finalize edebiliriz.
            During(PaymentCompleted,
                When(OrderCreatedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsOrderCreated = true;
                        context.Instance.OrderId = context.Data.OrderId;
                    })
                    .TransitionTo(OrderCreated)
                    .Finalize()
            );

            // **PaymentFailed** durumunda da eğer OrderFailEvent gelirse “OrderFailed” durumuna geçip finalize edebiliriz
            During(PaymentFailed,
                When(OrderFailEvent)
                    .Then(context =>
                    {
                        context.Instance.IsOrderFailed = true;
                    })
                    .TransitionTo(OrderFailed)
                    .Finalize()
            );


            // **OrderCreated** durumunda OrderCompletedEvent geldiğinde OrderCompleted durumuna geçiş
            During(OrderCreated,
                When(OrderCompletedEvent)
                    .Then(context =>
                    {
                        context.Instance.IsOrderCompleted = true;
                        context.Instance.AddressId = context.Data.AddressId;
                    })
                    .TransitionTo(OrderCompleted)
                    // Önce kullanıcı bilgilerini almak için User.API'ye mesaj gönder
                    .Send(new Uri($"queue:{RabbitMQSettings.GetUserDetailRequestEvent}"),
                          context => new GetUserDetailRequestEvent(context.Data.CorrelationId)
                          {
                              UserId = context.Instance.UserId
                          })
            );


            // **OrderCompleted** durumunda GetUserDetailResponseEvent'i ele alma
            During(OrderCompleted,
                When(GetUserDetailResponseEvent)
                    .Then(context =>
                    {
                        // Userss entity'sine göre kullanıcı bilgilerini Saga instance'ına kaydet
                        context.Instance.Username = context.Data.Username;
                        context.Instance.Name = context.Data.Name;
                        context.Instance.Surname = context.Data.Surname;
                        context.Instance.Email = context.Data.Email;
                        context.Instance.TelNo = context.Data.TelNo;
                        context.Instance.Birthdate = context.Data.Birthdate;
                    })
                    .TransitionTo(UserReceived)
                    // Kullanıcı bilgileri alındıktan sonra adres bilgilerini al
                    .Send(new Uri($"queue:{RabbitMQSettings.GetAddressDetailRequestEvent}"),
                          context => new GetAddressDetailRequestEvent(context.Data.CorrelationId)
                          {
                              AddressId = context.Instance.AddressId
                          })
            );

            // **UserReceived** durumunda GetAddressDetailResponseEvent'i ele alma
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

                        // Burada kullanıcı ve adres bilgileri alındıktan sonra diğer işlemler yapılabilir
                        // Örneğin: Fatura oluşturma, bildirim gönderme vb.
                    })
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
    }
}
