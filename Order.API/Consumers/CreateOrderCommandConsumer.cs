using MassTransit;
using Order.API.Data;
using Order.API.Data.Entities;
using Shared.Events.OrderCreatedEvent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Order.API.Consumers
{
    public class CreateOrderCommandConsumer : IConsumer<CreateOrderCommand>
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<CreateOrderCommandConsumer> _logger;

        public CreateOrderCommandConsumer(OrderDbContext context, ILogger<CreateOrderCommandConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateOrderCommand> context)
        {
            _logger.LogInformation("Yeni sipariş oluşturma isteği alındı. KullanıcıId: {UserId}, SepetId: {BasketId}, ToplamFiyat: {TotalPrice}",
                                    context.Message.UserId, context.Message.BasketId, context.Message.TotalPrice);

            var order = new Orderss
            {
                UserId = context.Message.UserId,
                BasketId = context.Message.BasketId,
                TotalPrice = context.Message.TotalPrice,
                CretaedDate = DateTime.UtcNow,
                OrderStatus = Data.Enums.OrdeStatus.Suspend,
            };

            try
            {
                await _context.Orderss.AddAsync(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Sipariş başarıyla oluşturuldu. SiparişId: {OrderId}", order.ID);

                foreach (var item in context.Message.BasketItemMessages)
                {
                    var orderItem = new OrderItemss
                    {
                        OrderId = order.ID,
                        ProductId = item.ProductId,
                        Count = item.Count,
                        TotalPrice = item.Price * item.Count
                    };

                    await _context.OrderItems.AddAsync(orderItem);
                    _logger.LogInformation("Sipariş öğesi başarıyla eklendi. SiparişId: {OrderId}, ÜrünId: {ProductId}, Miktar: {Count}, ToplamFiyat: {TotalPrice}",
                                            order.ID, item.ProductId, item.Count, item.Price * item.Count);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Sipariş öğeleri başarıyla kaydedildi. SiparişId: {OrderId}", order.ID);

                await context.Publish(new OrderCompletedEvent(context.Message.CorrelationId)
                {
                    OrderId = order.ID,
                });

                _logger.LogInformation("OrderCompletedEvent başarıyla yayınlandı. SiparişId: {OrderId}", order.ID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş oluşturulurken bir hata oluştu. KullanıcıId: {UserId}, SepetId: {BasketId}",
                                  context.Message.UserId, context.Message.BasketId);
                throw;
            }
        }
    }
}
