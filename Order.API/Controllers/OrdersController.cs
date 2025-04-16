using Microsoft.AspNetCore.Mvc;
using Order.API.Data.Entities;
using Order.API.Services.OrderServices;
using Order.API.DTOS.OrdersDTO.Orders;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderServices orderServices, IMapper mapper, ILogger<OrdersController> logger)
        {
            _orderServices = orderServices;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderss>>> GetAllOrders()
        {
            _logger.LogInformation("Tüm siparişler alınıyor.");
            var orders = await _orderServices.GetAllAsync();
            _logger.LogInformation("Tüm siparişler başarıyla alındı.");
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Orderss>> GetOrderById(int id)
        {
            _logger.LogInformation("Sipariş alınıyor. SiparişId: {id}", id);
            var order = await _orderServices.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Sipariş bulunamadı. SiparişId: {id}", id);
                return NotFound("Order not found.");
            }

            _logger.LogInformation("Sipariş başarıyla alındı. SiparişId: {id}", id);
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrdersDTO orderDTO)
        {
            _logger.LogInformation("Yeni sipariş oluşturuluyor. SiparişBilgisi: {OrderDetails}", orderDTO.ToString());
            var order = _mapper.Map<Orderss>(orderDTO);
            await _orderServices.AddAsync(order);
            _logger.LogInformation("Sipariş başarıyla oluşturuldu. SiparişId: {id}", order.ID);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.ID }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrdersDTO orderDTO)
        {
            if (id != orderDTO.ID)
            {
                _logger.LogWarning("Sipariş ID uyuşmazlığı. URL'deki ID: {id}, DTO'daki ID: {orderItemId}", id, orderDTO.ID);
                return BadRequest("Order ID mismatch.");
            }

            _logger.LogInformation("Sipariş güncelleniyor. SiparişId: {id}", id);
            var existingOrder = await _orderServices.GetByIdAsync(id);
            if (existingOrder == null)
            {
                _logger.LogWarning("Güncellenmek istenen sipariş bulunamadı. SiparişId: {id}", id);
                return NotFound("Order not found.");
            }

            var updatedOrder = _mapper.Map(orderDTO, existingOrder);
            var result = await _orderServices.UpdateAsync(updatedOrder);

            if (!result)
            {
                _logger.LogWarning("Sipariş güncellenemedi. SiparişId: {id}", id);
                return BadRequest("Failed to update order.");
            }

            _logger.LogInformation("Sipariş başarıyla güncellendi. SiparişId: {id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            _logger.LogInformation("Sipariş siliniyor. SiparişId: {id}", id);
            var result = await _orderServices.RemoveAsync(id);
            if (!result)
            {
                _logger.LogWarning("Silinmek istenen sipariş bulunamadı. SiparişId: {id}", id);
                return NotFound("Order not found.");
            }

            _logger.LogInformation("Sipariş başarıyla silindi. SiparişId: {id}", id);
            return NoContent();
        }
    }
}
