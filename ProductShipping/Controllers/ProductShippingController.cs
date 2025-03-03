using Microsoft.AspNetCore.Mvc;
using ProductShipping.DTOS.ProductShipping;
using ProductShipping.service;

namespace ProductShipping.Controllers
{
    public class ProductShippingController :ControllerBase
    {
        private readonly IProductShippingService _productshippingService;
        public ProductShippingController(IProductShippingService productshippingService)
        {
            _productshippingService = productshippingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adresler = await _productshippingService.GetAllAsync();
            return Ok(adresler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var adres = await _productshippingService.GetByIdAsync(id);
            if (adres == null)
                return NotFound($"{id} ID'li kategori bulunamadı");
            return Ok(adres);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductShippingDTO createproductshippingDto)
        {
            var sonuc = await _productshippingService.AddAsync(createproductshippingDto);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.ProductId }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductShippingDTO updateproductshippingDto)
        {
            if (id != updateproductshippingDto.ShippingId)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            var sonuc = await _productshippingService.UpdateAsync(updateproductshippingDto);
            if (!sonuc)
                return NotFound($"{id} ID'li kategori bulunamadı");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sonuc = await _productshippingService.DeleteAsync(id);
            if (!sonuc)
                return NotFound($"{id} ID'li kategori bulunamadı");
            return NoContent();
        }
    }
}
