using Microsoft.AspNetCore.Mvc;
using Product.API.service.CategoryService;
using Product.API.DTOS.CategoryDTO.Category;
using Microsoft.AspNetCore.Authorization;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ICategoryService categoryService,
            ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm kategoriler istendi");
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("{Id} ID'li kategori istendi", id);
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("{Id} ID'li kategori bulunamadı", id);
                return NotFound($"{id} ID'li kategori bulunamadı");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
        {
            _logger.LogInformation("Yeni kategori oluşturma isteği alındı: {@CategoryData}", dto);

            try
            {
                var created = await _categoryService.AddAsync(dto);
                _logger.LogInformation("Kategori başarıyla oluşturuldu: ID={CategoryId}, Name={CategoryName}",
                    created.Id, created.Name);

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori oluşturulurken hata oluştu: {ErrorMessage}", ex.Message);
                return StatusCode(500, "Kategori oluşturulurken bir hata oluştu. Detaylar için loglara bakın.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO dto)
        {
            _logger.LogInformation("{Id} ID'li kategori güncelleme isteği alındı: {@CategoryData}", id, dto);

            if (id != dto.CategoryId)
            {
                _logger.LogWarning("URL'deki ID ({UrlId}) ile DTO'daki ID ({DtoId}) eşleşmiyor", id, dto.CategoryId);
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            }

            try
            {
                var result = await _categoryService.UpdateAsync(dto);
                if (!result)
                {
                    _logger.LogWarning("{Id} ID'li kategori güncellenirken bulunamadı", id);
                    return NotFound($"{id} ID'li kategori bulunamadı");
                }

                _logger.LogInformation("{Id} ID'li kategori başarıyla güncellendi", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Id} ID'li kategori güncellenirken hata oluştu: {ErrorMessage}", id, ex.Message);
                return StatusCode(500, "Kategori güncellenirken bir hata oluştu. Detaylar için loglara bakın.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("{Id} ID'li kategori silme isteği alındı", id);

            try
            {
                var result = await _categoryService.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("{Id} ID'li kategori silinirken bulunamadı", id);
                    return NotFound($"{id} ID'li kategori bulunamadı");
                }

                _logger.LogInformation("{Id} ID'li kategori başarıyla silindi", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Id} ID'li kategori silinirken hata oluştu: {ErrorMessage}", id, ex.Message);
                return StatusCode(500, "Kategori silinirken bir hata oluştu. Detaylar için loglara bakın.");
            }
        }
    }
}