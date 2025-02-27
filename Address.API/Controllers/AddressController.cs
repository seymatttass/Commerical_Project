using Address.API.DTOS.Address;
using Address.API.services;
using Microsoft.AspNetCore.Mvc;

namespace Address.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adresler = await _addressService.GetAllAsync();
            return Ok(adresler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var adres = await _addressService.GetByIdAsync(id);
            if (adres == null)
                return NotFound($"{id} ID'li adres bulunamadı");
            return Ok(adres);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressDTO createAddressDto)
        {
            var sonuc = await _addressService.AddAsync(createAddressDto);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAddressDTO updateAddressDto)
        {
            if (id != updateAddressDto.AddressId)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            var sonuc = await _addressService.UpdateAsync(updateAddressDto);
            if (!sonuc)
                return NotFound($"{id} ID'li adres bulunamadı");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sonuc = await _addressService.DeleteAsync(id);
            if (!sonuc)
                return NotFound($"{id} ID'li adres bulunamadı");
            return NoContent();
        }
    }
}