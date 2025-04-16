using Microsoft.AspNetCore.Mvc;
using Product.API.DTOS.ProductCategoryDTO.ProductCategory;
using Product.API.service.ProductCategoryService;
using Microsoft.Extensions.Logging;

namespace Product.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly ILogger<ProductCategoryController> _logger;

        public ProductCategoryController(IProductCategoryService productCategoryService, ILogger<ProductCategoryController> logger)
        {
            _productCategoryService = productCategoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm productCategory'ler alınıyor.");
            var produccategorys = await _productCategoryService.GetAllAsync();
            _logger.LogInformation("Tüm productCategory'ler başarıyla alındı.");
            return Ok(produccategorys);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("ProductCategory alınıyor. ProductCategoryId: {id}", id);
            var produccategory = await _productCategoryService.GetByIdAsync(id);
            if (produccategory == null)
            {
                _logger.LogWarning("ProductCategory bulunamadı. ProductCategoryId: {id}", id);
                return NotFound($"{id} ID'li productCategory bulunamadı");
            }
            _logger.LogInformation("ProductCategory başarıyla alındı. ProductCategoryId: {id}", id);
            return Ok(produccategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCategoryDTO createProductCategoryDto)
        {
            var sonuc = await _productCategoryService.AddAsync(createProductCategoryDto);
            _logger.LogInformation("ProductCategory başarıyla oluşturuldu. ProductCategoryId: {id}", sonuc.Id);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCategoryDTO updateProductCategoryDto)
        {
            if (id != updateProductCategoryDto.ProductCategoryId)
            {
                _logger.LogWarning("URL'deki ID ile DTO'daki ID eşleşmiyor. URL ID: {id}, DTO ID: {dtoId}", id, updateProductCategoryDto.ProductCategoryId);
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            }

            _logger.LogInformation("ProductCategory güncelleniyor. ProductCategoryId: {id}", id);
            var sonuc = await _productCategoryService.UpdateAsync(updateProductCategoryDto);
            if (!sonuc)
            {
                _logger.LogWarning("ProductCategory güncellenemedi. ProductCategoryId: {id}", id);
                return NotFound($"{id} ID'li productCategory bulunamadı");
            }
            _logger.LogInformation("ProductCategory başarıyla güncellendi. ProductCategoryId: {id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("ProductCategory siliniyor. ProductCategoryId: {id}", id);
            var sonuc = await _productCategoryService.DeleteAsync(id);
            if (!sonuc)
            {
                _logger.LogWarning("ProductCategory silinemedi. ProductCategoryId: {id}", id);
                return NotFound($"{id} ID'li productCategory bulunamadı");
            }
            _logger.LogInformation("ProductCategory başarıyla silindi. ProductCategoryId: {id}", id);
            return NoContent();
        }
    }
}
