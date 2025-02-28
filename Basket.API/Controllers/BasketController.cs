using Basket.API.DTOS;
using Basket.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var baskets = await _basketService.GetAllAsync();
            return Ok(baskets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var basket = await _basketService.GetByIdAsync(id);
            if (basket == null)
                return NotFound($"{id} ID'li basket bulunamadı.");
            return Ok(basket);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBasketDTO createBasketDto)
        {
            var result = await _basketService.AddAsync(createBasketDto);
            return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBasketDTO updateBasketDto)
        {
            if (id != updateBasketDto.Id)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor.");

            var result = await _basketService.UpdateAsync(updateBasketDto);
            if (!result)
                return NotFound($"{id} ID'li basket bulunamadı.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _basketService.DeleteAsync(id);
            if (!result)
                return NotFound($"{id} ID'li basket bulunamadı.");

            return NoContent();
        }
    }
}
