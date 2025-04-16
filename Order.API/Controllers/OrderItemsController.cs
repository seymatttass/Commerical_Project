using Microsoft.AspNetCore.Mvc;
using Order.API.Data.Entities;
using Order.API.Services.OrderItemServices;
using Order.API.DTOS.OrderItemDTO.OrderItem;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemServices _orderItemServices;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderItemsController> _logger;

        public OrderItemsController(IOrderItemServices orderItemServices, IMapper mapper, ILogger<OrderItemsController> logger)
        {
            _orderItemServices = orderItemServices;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemss>>> GetAllOrderItems()
        {
            _logger.LogInformation("Tüm sipariş öğeleri alınıyor.");
            var orderItems = await _orderItemServices.GetAllAsync();
            _logger.LogInformation("Tüm sipariş öğeleri başarıyla alındı.");
            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemss>> GetOrderItemById(int id)
        {
            _logger.LogInformation("Sipariş öğesi alınıyor. SiparişÖğesiId: {id}", id);
            var orderItem = await _orderItemServices.GetByIdAsync(id);
            if (orderItem == null)
            {
                _logger.LogWarning("Sipariş öğesi bulunamadı. SiparişÖğesiId: {id}", id);
                return NotFound("Order item not found.");
            }

            _logger.LogInformation("Sipariş öğesi başarıyla alındı. SiparişÖğesiId: {id}", id);
            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrderItem([FromBody] CreateOrderItemDTO orderItemDTO)
        {
            var orderItem = _mapper.Map<OrderItemss>(orderItemDTO);
            await _orderItemServices.AddAsync(orderItem);
            _logger.LogInformation("Sipariş öğesi başarıyla oluşturuldu. SiparişÖğesiId: {id}", orderItem.ID);
            return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.ID }, orderItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] UpdateOrderItemDTO orderItemDTO)
        {
            if (id != orderItemDTO.ID)
            {
                _logger.LogWarning("Sipariş öğesi ID uyumsuzluğu. URL'deki ID: {id}, DTO'daki ID: {orderItemId}", id, orderItemDTO.ID);
                return BadRequest("OrderItem ID mismatch.");
            }

            _logger.LogInformation("Sipariş öğesi güncelleniyor. SiparişÖğesiId: {id}", id);
            var existingOrderItem = await _orderItemServices.GetByIdAsync(id);
            if (existingOrderItem == null)
            {
                _logger.LogWarning("Güncellenmek istenen sipariş öğesi bulunamadı. SiparişÖğesiId: {id}", id);
                return NotFound("Order item not found.");
            }

            var updatedOrderItem = _mapper.Map(orderItemDTO, existingOrderItem);
            var result = await _orderItemServices.UpdateAsync(updatedOrderItem);

            if (!result)
            {
                _logger.LogWarning("Sipariş öğesi güncellenemedi. SiparişÖğesiId: {id}", id);
                return BadRequest("Failed to update order item.");
            }

            _logger.LogInformation("Sipariş öğesi başarıyla güncellendi. SiparişÖğesiId: {id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            _logger.LogInformation("Sipariş öğesi siliniyor. SiparişÖğesiId: {id}", id);
            var result = await _orderItemServices.RemoveAsync(id);
            if (!result)
            {
                _logger.LogWarning("Silinmek istenen sipariş öğesi bulunamadı. SiparişÖğesiId: {id}", id);
                return NotFound("Order item not found.");
            }

            _logger.LogInformation("Sipariş öğesi başarıyla silindi. SiparişÖğesiId: {id}", id);
            return NoContent();
        }
    }
}
