using Microsoft.AspNetCore.Mvc;
using Order.API.Data.Entities;
using Order.API.Services.OrderServices;
using Order.API.DTOS.OrdersDTO.Orders;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;

        public OrdersController(IOrderServices orderServices, IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderss>>> GetAllOrders()
        {
            var orders = await _orderServices.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Orderss>> GetOrderById(int id)
        {
            var order = await _orderServices.GetByIdAsync(id);
            if (order == null)
                return NotFound("Order not found.");

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrdersDTO orderDTO)
        {
            var order = _mapper.Map<Orderss>(orderDTO);
            await _orderServices.AddAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.ID }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrdersDTO orderDTO)
        {
            if (id != orderDTO.ID)
                return BadRequest("Order ID mismatch.");

            var existingOrder = await _orderServices.GetByIdAsync(id);
            if (existingOrder == null)
                return NotFound("Order not found.");

            var updatedOrder = _mapper.Map(orderDTO, existingOrder);
            var result = await _orderServices.UpdateAsync(updatedOrder);

            if (!result) return BadRequest("Failed to update order.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderServices.RemoveAsync(id);
            if (!result)
                return NotFound("Order not found.");

            return NoContent();
        }
    }
}
