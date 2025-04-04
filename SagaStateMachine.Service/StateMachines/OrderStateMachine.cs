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
        public State ProductAdded { get; private set; }
        public State StockReserved { get; private set; }
        public State StockNotReserved { get; private set; }
        public State StockReduced { get; private set; }
        public State PaymentStarted { get; private set; }
        public State PaymentCompleted { get; private set; }
        public State PaymentFailed { get; private set; }
        public State OrderCreated { get; private set; }
        public State OrderCompleted { get; private set; }
        public State OrderFailed { get; private set; }

        public Event<ProductAddedToBasketRequestEvent> ProductAddedToBasketRequestEvent { get; private set; }
        public Event<StockReservedEvent> StockReservedEvent { get; private set; }
        public Event<StockNotReservedEvent> StockNotReservedEvent { get; private set; }
        public Event<PaymentStartedEvent> PaymentStartedEvent { get; private set; }
        public Event<PaymentCompletedEvent> PaymentCompletedEvent { get; private set; }
        public Event<PaymentFailedEvent> PaymentFailedEvent { get; private set; }
        public Event<StockReductionEvent> StockReductionEvent { get; private set; }
        public Event<OrderFailEvent> OrderFailEvent { get; private set; }
        public Event<OrderCompletedEvent> OrderCompletedEvent { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => ProductAddedToBasketRequestEvent,
                x => x.CorrelateById(e => e.Message.CorrelationId).SelectId(e => Guid.NewGuid()));

            Event(() => StockReservedEvent,
                x => x.CorrelateById(e => e.Message.CorrelationId));

            Event(() => StockNotReservedEvent,
                x => x.CorrelateById(e => e.Message.CorrelationId));

            Event(() => PaymentStartedEvent,
                x => x.CorrelateById(e => e.Message.CorrelationId));

            Event(() => PaymentCompletedEvent,
                x => x.CorrelateById(e => e.Message.CorrelationId));

            Event(() => PaymentFailedEvent,
                x => x.CorrelateById(e => e.Message.CorrelationId));

            Event(() => StockReductionEvent,
                x => x.CorrelateById(e => e.Message.CorrelationId));

            Event(() => OrderFailEvent,
                x => x.CorrelateById(e => e.Message.CorrelationId));

            Event(() => OrderCompletedEvent,
                x => x.CorrelateById(e => e.Message.CorrelationId));

            Initially(
                When(ProductAddedToBasketRequestEvent)
                    .Then(context =>
                    {
                        context.Instance.UserId = context.Data.UserId;
                        context.Instance.CreatedDate = DateTime.UtcNow;
                        context.Instance.TotalPrice = context.Data.Price * context.Data.Count;
                        context.Instance.ProductId = context.Data.ProductId;
                        context.Instance.Count = context.Data.Count;
                        context.Instance.Price = context.Data.Price;
                        context.Instance.BasketId = context.Data.BasketId;
                    })
                    .TransitionTo(ProductAdded)
                    .Send(new Uri($"queue:{RabbitMQSettings.Stock_CheckStockQueue}"),
                        context => new StockCheckedEvent(context.Instance.CorrelationId)
                        {
                            ProductId = context.Instance.ProductId,
                            Count = context.Instance.Count
                        })
            );

            During(ProductAdded,
                When(StockReservedEvent)
                    .TransitionTo(StockReserved)
                    .Send(new Uri($"queue:{RabbitMQSettings.Payment_PaymentStartedQueue}"),
                        context => new PaymentStartedEvent(context.Instance.CorrelationId)
                        {
                            TotalPrice = context.Instance.TotalPrice,
                            BasketItemMessages = new List<BasketItemMessage>
                            {
                                new BasketItemMessage
                                {
                                    ProductId = context.Instance.ProductId,
                                    Count = context.Instance.Count,
                                    Price = context.Instance.Price
                                }
                            }
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
                    .Then(context =>
                    {
                  
                    })
                    .TransitionTo(PaymentCompleted)
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderCreatedQueue}"),
                        context => new CreateOrderCommand(context.Instance.CorrelationId)
                        {
                            UserId = context.Instance.UserId,
                            BasketId = context.Instance.BasketId,
                            TotalPrice = context.Instance.TotalPrice,
                            BasketItemMessages = new List<BasketItemMessage>
                            {
                                new BasketItemMessage
                                {
                                    ProductId = context.Instance.ProductId,
                                    Count = context.Instance.Count,
                                    Price = context.Instance.Price
                                }
                            }
                        }),

                When(PaymentFailedEvent)
                    .TransitionTo(PaymentFailed)
                    .Send(new Uri($"queue:{RabbitMQSettings.Order_OrderFailedQueue}"),
                        context => new OrderFailEvent(context.Instance.CorrelationId)
                        {
                            OrderId = context.Instance.OrderId,
                            Message = context.Data.Message
                        })
            );

            During(PaymentCompleted,
                When(OrderCompletedEvent)
                    .Then(context =>
                    {
                        context.Instance.OrderId = context.Data.OrderId;
                    })
                    .TransitionTo(OrderCompleted)
                   .Send(new Uri($"queue:{RabbitMQSettings.Stock_ReductionQueue}"),
                        context => new StockReductionEvent(context.Instance.CorrelationId)
                         {
                              OrderId = context.Instance.OrderId,
                              OrderItems = new List<BasketItemMessage> // ← Burası önemli!
                         {
                new BasketItemMessage
            {
                ProductId = context.Instance.ProductId,
                Count = context.Instance.Count,
                Price = context.Instance.Price
            }
        }
    })
            );

            During(OrderCompleted,
                When(StockReductionEvent)
                    .TransitionTo(StockReduced)
                    .Finalize()
            );

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

