using Microsoft.AspNetCore.Mvc;
using Order.API.Data.Entities;
using Order.API.Services.OrderItemServices;
using Order.API.DTOS.OrderItemDTO.OrderItem;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemServices _orderItemServices;
        private readonly IMapper _mapper;

        public OrderItemsController(IOrderItemServices orderItemServices, IMapper mapper)
        {
            _orderItemServices = orderItemServices;
            _mapper = mapper;
        }

        // GET: api/orderitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemss>>> GetAllOrderItems()
        {
            var orderItems = await _orderItemServices.GetAllAsync();
            return Ok(orderItems);
        }

        // GET: api/orderitems/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemss>> GetOrderItemById(int id)
        {
            var orderItem = await _orderItemServices.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound("Order item not found.");

            return Ok(orderItem);
        }

        // POST: api/orderitems
        [HttpPost]
        public async Task<ActionResult> CreateOrderItem([FromBody] CreateOrderItemDTO orderItemDTO)
        {
            var orderItem = _mapper.Map<OrderItemss>(orderItemDTO);
            await _orderItemServices.AddAsync(orderItem);
            return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.ID }, orderItem);
        }

        // PUT: api/orderitems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] UpdateOrderItemDTO orderItemDTO)
        {
            if (id != orderItemDTO.ID)
                return BadRequest("OrderItem ID mismatch.");

            var existingOrderItem = await _orderItemServices.GetByIdAsync(id);
            if (existingOrderItem == null)
                return NotFound("Order item not found.");

            var updatedOrderItem = _mapper.Map(orderItemDTO, existingOrderItem);
            var result = await _orderItemServices.UpdateAsync(updatedOrderItem);

            if (!result) return BadRequest("Failed to update order item.");
            return NoContent();
        }

        // DELETE: api/orderitems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var result = await _orderItemServices.RemoveAsync(id);
            if (!result)
                return NotFound("Order item not found.");

            return NoContent();
        }
    }
}
