using InvoiceDetails.DTOS.InvoiceDetails;
using InvoiceDetails.services;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDetails.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceDetailsController : ControllerBase
    {


        private readonly IInvoiceDetailsService _invoiceDetailsService;
        public InvoiceDetailsController(IInvoiceDetailsService invoiceDetailsService)
        {
            _invoiceDetailsService = invoiceDetailsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adresler = await _invoiceDetailsService.GetAllAsync();
            return Ok(adresler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var adres = await _invoiceDetailsService.GetByIdAsync(id);
            if (adres == null)
                return NotFound($"{id} ID'li invoiceDetails bulunamadı");
            return Ok(adres);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDetailsDTO createInvoiceDetailsDto)
        {
            var sonuc = await _invoiceDetailsService.AddAsync(createInvoiceDetailsDto);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceDetailsDTO updateInvoiceDetailsDto)
        {
            if (id != updateInvoiceDetailsDto.InvoiceDetailsId)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            var sonuc = await _invoiceDetailsService.UpdateAsync(updateInvoiceDetailsDto);
            if (!sonuc)
                return NotFound($"{id} ID'li invoiceDetails bulunamadı");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sonuc = await _invoiceDetailsService.DeleteAsync(id);
            if (!sonuc)
                return NotFound($"{id} ID'li invoiceDetails bulunamadı");
            return NoContent();
        }
    }

}

