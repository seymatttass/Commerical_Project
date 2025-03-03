using Microsoft.AspNetCore.Mvc;
using ProductShippinng.API.DTOS.ProductShipping;
using ProductShippinng.API.services;

namespace ProductShippinng.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductShippingController : ControllerBase
    {

        private readonly IProductShippingService _productShippingService;
        public ProductShippingController(IProductShippingService productShippingService)
        {
            _productShippingService = productShippingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productShippings = await _productShippingService.GetAllAsync();
            return Ok(productShippings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var productShippings = await _productShippingService.GetByIdAsync(id);
            if (productShippings == null)
                return NotFound($"{id} ID'li productShipping bulunamadı");
            return Ok(productShippings);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductShippingDTO createProductShippingDto)
        {
            var sonuc = await _productShippingService.AddAsync(createProductShippingDto);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductShippingDTO updateProductShippingDto)
        {
            if (id != updateProductShippingDto.ProductShippingId)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            var sonuc = await _productShippingService.UpdateAsync(updateProductShippingDto);
            if (!sonuc)
                return NotFound($"{id} ID'li productShipping bulunamadı");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sonuc = await _productShippingService.DeleteAsync(id);
            if (!sonuc)
                return NotFound($"{id} ID'li productShipping bulunamadı");
            return NoContent();
        }
    }
}
