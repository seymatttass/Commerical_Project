using Payment.API.DTOS.Payments;
using Payment.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Payment.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm ödemeler alınıyor.");
            var payments = await _paymentService.GetAllAsync();
            _logger.LogInformation("Tüm ödemeler başarıyla alındı.");
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Ödeme alınıyor. ÖdemeId: {id}", id);
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
            {
                _logger.LogWarning("Ödeme bulunamadı. ÖdemeId: {id}", id);
                return NotFound($"{id} ID'li ödeme bulunamadı.");
            }

            _logger.LogInformation("Ödeme başarıyla alındı. ÖdemeId: {id}", id);
            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDTO createPaymentDto)
        {
            _logger.LogInformation("Yeni ödeme oluşturuluyor. ÖdemeBilgisi: {PaymentDetails}", createPaymentDto.ToString());
            var result = await _paymentService.AddAsync(createPaymentDto);
            _logger.LogInformation("Ödeme başarıyla oluşturuldu. ÖdemeId: {id}", result.ID);
            return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentDTO updatePaymentDto)
        {
            if (id != updatePaymentDto.Id)
            {
                _logger.LogWarning("Ödeme ID uyuşmazlığı. URL'deki ID: {id}, DTO'daki ID: {paymentId}", id, updatePaymentDto.Id);
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor.");
            }

            _logger.LogInformation("Ödeme güncelleniyor. ÖdemeId: {id}", id);
            var result = await _paymentService.UpdateAsync(updatePaymentDto);
            if (!result)
            {
                _logger.LogWarning("Ödeme güncellenemedi. ÖdemeId: {id}", id);
                return NotFound($"{id} ID'li ödeme bulunamadı.");
            }

            _logger.LogInformation("Ödeme başarıyla güncellendi. ÖdemeId: {id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Ödeme siliniyor. ÖdemeId: {id}", id);
            var result = await _paymentService.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning("Silinmek istenen ödeme bulunamadı. ÖdemeId: {id}", id);
                return NotFound($"{id} ID'li ödeme bulunamadı.");
            }

            _logger.LogInformation("Ödeme başarıyla silindi. ÖdemeId: {id}", id);
            return NoContent();
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrderId(int orderId)
        {
            _logger.LogInformation("Siparişe ait ödemeler alınıyor. SiparişId: {orderId}", orderId);
            var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
            _logger.LogInformation("Siparişe ait ödemeler başarıyla alındı. SiparişId: {orderId}", orderId);
            return Ok(payments);
        }

        [HttpGet("basket/{basketId}")]
        public async Task<IActionResult> HasPaymentForBasket(int basketId)
        {
            _logger.LogInformation("Sepete ait ödeme kontrol ediliyor. SepetId: {basketId}", basketId);
            var hasPayment = await _paymentService.HasPaymentForBasketAsync(basketId);
            _logger.LogInformation("Sepet için ödeme durumu kontrolü tamamlandı. SepetId: {basketId}, ÖdemeDurumu: {hasPayment}", basketId, hasPayment);
            return Ok(new { hasPayment });
        }
    }
}
