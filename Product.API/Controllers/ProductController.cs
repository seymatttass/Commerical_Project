using Microsoft.AspNetCore.Mvc;
using Product.API.DTOS.ProductDTO;
using Product.API.DTOS.ProductDTO.Product;
using Product.API.service;
using Product.API.service.ProductService;
using Microsoft.Extensions.Logging;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm ürünler alınıyor.");
            var product = await _productService.GetAllAsync();
            _logger.LogInformation("Tüm ürünler başarıyla alındı.");
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Ürün alınıyor. ÜrünId: {id}", id);
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Ürün bulunamadı. ÜrünId: {id}", id);
                return NotFound($"{id} ID'li kategori bulunamadı");
            }
            _logger.LogInformation("Ürün başarıyla alındı. ÜrünId: {id}", id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO createProductDto)
        {
            _logger.LogInformation("Yeni ürün oluşturuluyor: {ProductName}", createProductDto.Name);
            var sonuc = await _productService.AddAsync(createProductDto);
            _logger.LogInformation("Ürün başarıyla oluşturuldu. ÜrünId: {id}", sonuc.Id);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO updateProductDto)
        {
            if (id != updateProductDto.ProductId)
            {
                _logger.LogWarning("URL'deki ID ile DTO'daki ID eşleşmiyor. URL ID: {id}, DTO ID: {productId}", id, updateProductDto.ProductId);
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            }

            _logger.LogInformation("Ürün güncelleniyor. ÜrünId: {id}", id);
            var sonuc = await _productService.UpdateAsync(updateProductDto);
            if (!sonuc)
            {
                _logger.LogWarning("Ürün güncellenemedi. ÜrünId: {id}", id);
                return NotFound($"{id} ID'li kategori bulunamadı");
            }
            _logger.LogInformation("Ürün başarıyla güncellendi. ÜrünId: {id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Ürün siliniyor. ÜrünId: {id}", id);
            var sonuc = await _productService.DeleteAsync(id);
            if (!sonuc)
            {
                _logger.LogWarning("Ürün silinemedi. ÜrünId: {id}", id);
                return NotFound($"{id} ID'li kategori bulunamadı");
            }
            _logger.LogInformation("Ürün başarıyla silindi. ÜrünId: {id}", id);
            return NoContent();
        }
    }
}
