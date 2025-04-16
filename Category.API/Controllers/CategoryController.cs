using Microsoft.AspNetCore.Mvc;
using Category.API.DTOS.Category;
using Category.API.services;

namespace Category.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ICategoryService categoryService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm kategoriler alınıyor.");
            var categories = await _categoryService.GetAllAsync();
            _logger.LogInformation("Tüm kategoriler başarıyla alındı.");
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Kategori bilgisi alınıyor. KategoriId: {id}", id);
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Kategori bulunamadı. KategoriId: {id}", id);
                return NotFound($"{id} ID'li kategori bulunamadı");
            }

            _logger.LogInformation("Kategori başarıyla alındı. KategoriId: {id}", id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO createCategoryDto)
        {
            _logger.LogInformation("Yeni kategori oluşturma isteği alındı: {CategoryName}", createCategoryDto.Name);

            var createdCategory = await _categoryService.AddAsync(createCategoryDto);

            _logger.LogInformation("Kategori başarıyla oluşturuldu. ID: {CategoryId}", createdCategory.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO updateCategoryDto)
        {
            if (id != updateCategoryDto.CategoryId)
            {
                _logger.LogWarning("URL'deki ID ile DTO'daki ID eşleşmiyor. URL ID: {id}, DTO ID: {categoryId}", id, updateCategoryDto.CategoryId);
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            }

            _logger.LogInformation("Kategori güncelleniyor. KategoriId: {id}", id);
            var result = await _categoryService.UpdateAsync(updateCategoryDto);
            if (!result)
            {
                _logger.LogWarning("Kategori bulunamadı. KategoriId: {id}", id);
                return NotFound($"{id} ID'li kategori bulunamadı");
            }

            _logger.LogInformation("Kategori başarıyla güncellendi. KategoriId: {id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Kategori siliniyor. KategoriId: {id}", id);
            var result = await _categoryService.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning("Kategori bulunamadı. KategoriId: {id}", id);
                return NotFound($"{id} ID'li kategori bulunamadı");
            }

            _logger.LogInformation("Kategori başarıyla silindi. KategoriId: {id}", id);
            return NoContent();
        }
    }
}
