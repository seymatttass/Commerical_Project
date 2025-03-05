using Invoice.API.Data.Entities;
using Invoice.API.DTOS.Invoice;
using Invoice.API.services;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.API.Controllers
{
        [ApiController]
        [Route("[controller]")]
        public class InvoiceController : ControllerBase
        {
            private readonly IInvoiceService _invoiceService;
            public InvoiceController(IInvoiceService invoiceService)
            {
                  _invoiceService = invoiceService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var invoices = await _invoiceService.GetAllAsync();
                return Ok(invoices);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var invoice = await _invoiceService.GetByIdAsync(id);
                if (invoice == null)
                    return NotFound($"{id} ID'li invoice bulunamadı");
                return Ok(invoice);
        }
        [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateInvoiceDTO createInvoiceDto)
            {
                var sonuc = await _invoiceService.AddAsync(createInvoiceDto);
                return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceDTO updateInvoiceDto)
            {
                if (id != updateInvoiceDto.InvoiceId)
                    return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
                var sonuc = await _invoiceService.UpdateAsync(updateInvoiceDto);
                if (!sonuc)
                    return NotFound($"{id} ID'li invoice bulunamadı");
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var sonuc = await _invoiceService.DeleteAsync(id);
                if (!sonuc)
                    return NotFound($"{id} ID'li invoice bulunamadı");
                return NoContent();
            }
        }
}

