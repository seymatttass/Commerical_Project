using Microsoft.AspNetCore.Mvc;
using Product.API.service.CategoryService;
using Product.API.DTOS.CategoryDTO.Category;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/categories")] // => POST /api/categories, GET /api/categories/{id} vb.
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound($"{id} ID'li kategori bulunamadı");
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
        {
            var created = await _categoryService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO dto)
        {
            if (id != dto.CategoryId)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");

            var result = await _categoryService.UpdateAsync(dto);
            if (!result)
                return NotFound($"{id} ID'li kategori bulunamadı");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result)
                return NotFound($"{id} ID'li kategori bulunamadı");

            return NoContent();
        }
    }
}

