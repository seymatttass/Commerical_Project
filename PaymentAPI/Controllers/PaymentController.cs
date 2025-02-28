using Payment.API.DTOS.Payments;
using Payment.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Payment.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound($"{id} ID'li ödeme bulunamadı.");
            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDTO createPaymentDto)
        {
            var result = await _paymentService.AddAsync(createPaymentDto);
            return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentDTO updatePaymentDto)
        {
            if (id != updatePaymentDto.Id)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor.");

            var result = await _paymentService.UpdateAsync(updatePaymentDto);
            if (!result)
                return NotFound($"{id} ID'li ödeme bulunamadı.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _paymentService.DeleteAsync(id);
            if (!result)
                return NotFound($"{id} ID'li ödeme bulunamadı.");

            return NoContent();
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrderId(int orderId)
        {
            var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
            return Ok(payments);
        }

        [HttpGet("basket/{basketId}")]
        public async Task<IActionResult> HasPaymentForBasket(int basketId)
        {
            var hasPayment = await _paymentService.HasPaymentForBasketAsync(basketId);
            return Ok(new { hasPayment });
        }
    }
}
