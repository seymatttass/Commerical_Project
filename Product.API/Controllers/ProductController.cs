using Microsoft.AspNetCore.Mvc;
using Product.API.DTOS.Product;
using Product.API.service;

namespace Product.API.Controllers
{ 

        [ApiController]
        [Route("[controller]")]
        public class ProductController : ControllerBase
        {
            private readonly IProductService _productService;
            public ProductController(IProductService productService)
            {
               _productService = productService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var product = await _productService.GetAllAsync();
                return Ok(product);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                    return NotFound($"{id} ID'li kategori bulunamadı");
                return Ok(product);
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateProductDTO createProductDto)
            {
                var sonuc = await _productService.AddAsync(createProductDto);
                return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO updateProductDto)
            {
                if (id != updateProductDto.ProductId)
                    return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
                var sonuc = await _productService.UpdateAsync(updateProductDto);
                if (!sonuc)
                    return NotFound($"{id} ID'li kategori bulunamadı");
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var sonuc = await _productService.DeleteAsync(id);
                if (!sonuc)
                    return NotFound($"{id} ID'li kategori bulunamadı");
                return NoContent();
            }
        }
}

