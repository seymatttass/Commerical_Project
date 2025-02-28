using Basket.API.DTOS;
using Basket.API.Services;
using Microsoft.AspNetCore.Mvc;
namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/basket-items")]
    public class BasketItemController : ControllerBase
    {
        private readonly IBasketItemService _basketItemService;

        public BasketItemController(IBasketItemService basketItemService)
        {
            _basketItemService = basketItemService;
        }


        [HttpGet("basket/{basketId}")]
        public async Task<IActionResult> GetByBasketId(int basketId)
        {
            var items = await _basketItemService.GetByBasketIdAsync(basketId);
            return Ok(items);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _basketItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound($"Basket item {id} bulunamadı.");

            return Ok(item);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBasketItemDTO createBasketItemDto)
        {
            var result = await _basketItemService.AddAsync(createBasketItemDto);
            return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBasketItemDTO updateBasketItemDto)
        {
            if (id != updateBasketItemDto.Id)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor.");

            var result = await _basketItemService.UpdateAsync(updateBasketItemDto);
            if (!result)
                return NotFound($"Basket item {id} bulunamadı.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _basketItemService.DeleteAsync(id);
            if (!result)
                return NotFound($"Basket item {id} bulunamadı.");

            return NoContent();
        }
    }
}
