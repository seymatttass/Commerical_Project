using Category.API.DTOS.Category;
using Category.API.services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Events.CategoryEvents;
using System;
using System.Threading.Tasks;

namespace Category.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IPublishEndpoint _publishEndpoint;


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound($"{id} ID'li kategori bulunamadı");
            return Ok(category);
        }

        public CategoryController(ICategoryService categoryService, IPublishEndpoint publishEndpoint)
        {
            _categoryService = categoryService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO createCategoryDto)
        {
            var result = await _categoryService.AddAsync(createCategoryDto);

            if (result != null)
            {
                var categoryCreatedEvent = new CategoryCreatedEvent(Guid.NewGuid()) // Burada CorrelationId gerekmiyor
                {
                    CategoryId = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    Active = result.Active
                };

                await _publishEndpoint.Publish(categoryCreatedEvent);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO updateCategoryDto)
        {
            if (id != updateCategoryDto.CategoryId)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");

            var updated = await _categoryService.UpdateAsync(updateCategoryDto);
            if (!updated)
                return NotFound($"{id} ID'li kategori bulunamadı");

            var categoryUpdatedEvent = new CategoryUpdatedEvent(Guid.NewGuid())
            {
                CategoryId = updateCategoryDto.CategoryId,
                Name = updateCategoryDto.Name,
                Description = updateCategoryDto.Description,
                Active = updateCategoryDto.Active
            };

            await _publishEndpoint.Publish(categoryUpdatedEvent);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"{id} ID'li kategori bulunamadı");

            var categoryDeletedEvent = new CategoryDeletedEvent(Guid.NewGuid())
            {
                CategoryId = id
            };

            await _publishEndpoint.Publish(categoryDeletedEvent);

            return NoContent();
        }
    }
}
