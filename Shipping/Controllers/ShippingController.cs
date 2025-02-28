using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Shipping.API.DTOS.Shipping;
using Shipping.API.service;

namespace Shipping.API.Controllers
{
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _categoryService;
        public ShippingController(IShippingService addressService)
        {
            _categoryService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adresler = await _categoryService.GetAllAsync();
            return Ok(adresler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var adres = await _categoryService.GetByIdAsync(id);
            if (adres == null)
                return NotFound($"{id} ID'li kategori bulunamadı");
            return Ok(adres);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShippingDTO createAddressDto)
        {
            var sonuc = await _categoryService.AddAsync(createAddressDto);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateShippingDTO updateAddressDto)
        {
            if (id != updateAddressDto.ShippingId)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            var sonuc = await _categoryService.UpdateAsync(updateAddressDto);
            if (!sonuc)
                return NotFound($"{id} ID'li kategori bulunamadı");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sonuc = await _categoryService.DeleteAsync(id);
            if (!sonuc)
                return NotFound($"{id} ID'li kategori bulunamadı");
            return NoContent();
        }
    }
}
