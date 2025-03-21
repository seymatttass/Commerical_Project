//using MassTransit;
//using Shared.Events.ShippingEvents;
//using System;
//using System.Threading.Tasks;
//using Shared.Events.OrderCreatedEvent;
//using Order.API.Services.OrderServices;
//using Order.API.Data.Entities;
//using System.Linq;
//using Order.API.Data.Repository.OrderItem;

//namespace Order.API.Consumers
//{
//    public class ShippingFailedEventConsumer : IConsumer<ShippingFailedEvent>
//    {
//        private readonly IOrderServices _orderService;
//        private readonly IOrderItemRepository _orderItemRepository;
//        private readonly IPublishEndpoint _publishEndpoint;

//        public ShippingFailedEventConsumer(
//            IOrderServices orderService,
//            IOrderItemRepository orderItemRepository,
//            IPublishEndpoint publishEndpoint)
//        {
//            _orderService = orderService;
//            _orderItemRepository = orderItemRepository;
//            _publishEndpoint = publishEndpoint;
//        }

//        public async Task Consume(ConsumeContext<ShippingFailedEvent> context)
//        {
//            try
//            {
//                // ShippingId ile OrderItems'ı bul
//                var orderItems = await FindOrderItemsByShippingIdAsync(context.Message.ShippingId);

//                if (orderItems != null)
//                {
//                    // OrderItems üzerinden Order'ı bul
//                    var order = await _orderService.GetByIdAsync(orderItems.ID);

//                    if (order != null)
//                    {
//                        // OrderFailEvent'i publish et (saga için)
//                        await _publishEndpoint.Publish(new OrderFailEvent(context.Message.CorrelationId)
//                        {
//                            OrderId = order.ID,
//                            Message = $"Kargo işlemi başarısız oldu. Kargo Şirketi: {context.Message.CargoCompanyName}"
//                        });
//                    }
//                }
//            }
//            catch (Exception)
//            {
//                // Exception loglaması kaldırıldı
//            }
//        }

//        // ShippingId'ye göre OrderItems'ı bulma
//        private async Task<OrderItemss> FindOrderItemsByShippingIdAsync(int shippingId)
//        {
//            var orderItems = await _orderItemRepository.FindAsync(oi => oi.ShippingId == shippingId);
//            return orderItems.FirstOrDefault();
//        }
//    }
//}