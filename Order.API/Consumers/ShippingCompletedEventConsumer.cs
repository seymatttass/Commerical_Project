//using MassTransit;
//using Shared.Events.ShippingEvents;
//using System;
//using System.Threading.Tasks;
//using Order.API.Services.OrderServices;
//using Order.API.Data.Repository.OrderItem;
//using Order.API.Data.Entities;
//using Order.API.Data.Enums;
//using System.Linq;

//namespace Order.API.Consumers
//{
//    public class ShippingCompletedEventConsumer : IConsumer<ShippingCompletedEvent>
//    {
//        private readonly IOrderServices _orderService;
//        private readonly IOrderItemRepository _orderItemRepository;

//        public ShippingCompletedEventConsumer(
//            IOrderServices orderService,
//            IOrderItemRepository orderItemRepository)
//        {
//            _orderService = orderService;
//            _orderItemRepository = orderItemRepository;
//        }

//        public async Task Consume(ConsumeContext<ShippingCompletedEvent> context)
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
//                        // Sipariş durumunu "Kargoya Verildi" olarak güncelle
//                        order.OrderStatus = OrdeStatus.Shipped;

//                        await _orderService.UpdateAsync(order);
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