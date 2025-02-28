using Microsoft.AspNetCore.Mvc;
using ProductCategory.DTOS.ProductCategory;
using ProductCategory.service;

namespace ProductCategory.Controllers
{

        [ApiController]
        [Route("[controller]")]
        public class ProductCategoryController : ControllerBase
        {
            private readonly IProductCategoryService _productCategoryService;
            public ProductCategoryController(IProductCategoryService productCategoryService)
            {
                _productCategoryService = productCategoryService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var produccategorys = await _productCategoryService.GetAllAsync();
                return Ok(produccategorys);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var produccategory = await _productCategoryService.GetByIdAsync(id);
                if (produccategory == null)
                    return NotFound($"{id} ID'li productCategory bulunamadı");
                return Ok(produccategory);
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateProductCategoryDTO createProductCategoryDto)
            {
                var sonuc = await _productCategoryService.AddAsync(createProductCategoryDto);
                return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCategoryDTO updateProductCategoryDto)
            {
                if (id != updateProductCategoryDto.ProductCategoryId)
                    return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
                var sonuc = await _productCategoryService.UpdateAsync(updateProductCategoryDto);
                if (!sonuc)
                    return NotFound($"{id} ID'li productCategory bulunamadı");
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var sonuc = await _productCategoryService.DeleteAsync(id);
                if (!sonuc)
                    return NotFound($"{id} ID'li productCategory bulunamadı");
                return NoContent();
            }
        }
 }

