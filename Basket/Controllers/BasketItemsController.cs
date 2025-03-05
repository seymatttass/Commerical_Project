using Microsoft.AspNetCore.Mvc;
using Basket.API.Data.Entities;
using Basket.API.Services;
using Basket.API.DTOS;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemController : ControllerBase
    {
        private readonly IBasketItemService _basketItemService;
        private readonly IMapper _mapper;

        public BasketItemController(IBasketItemService basketItemService, IMapper mapper)
        {
            _basketItemService = basketItemService;
            _mapper = mapper;
        }

        // GET: api/basketitem/basket/{basketId}
        [HttpGet("basket/{basketId}")]
        public async Task<ActionResult<IEnumerable<BasketItem>>> GetBasketItemsByBasketId(int basketId)
        {
            var basketItems = await _basketItemService.GetByBasketIdAsync(basketId);
            return Ok(basketItems);
        }

        // GET: api/basketitem/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketItem>> GetBasketItemById(int id)
        {
            var basketItem = await _basketItemService.GetByIdAsync(id);
            if (basketItem == null)
                return NotFound("Basket item not found.");

            return Ok(basketItem);
        }

        // POST: api/basketitem
        [HttpPost]
        public async Task<ActionResult> CreateBasketItem([FromBody] CreateBasketItemDTO basketItemDTO)
        {
            var basketItem = await _basketItemService.AddAsync(basketItemDTO);
            return CreatedAtAction(nameof(GetBasketItemById), new { id = basketItem.ID }, basketItem);
        }

        // PUT: api/basketitem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBasketItem(int id, [FromBody] UpdateBasketItemDTO basketItemDTO)
        {
            if (id != basketItemDTO.Id)
                return BadRequest("BasketItem ID mismatch.");

            var existingItem = await _basketItemService.GetByIdAsync(id);
            if (existingItem == null)
                return NotFound("Basket item not found.");

            var result = await _basketItemService.UpdateAsync(basketItemDTO);
            if (!result) return BadRequest("Failed to update basket item.");

            return NoContent();
        }

        // DELETE: api/basketitem/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketItem(int id)
        {
            var result = await _basketItemService.DeleteAsync(id);
            if (!result)
                return NotFound("Basket item not found.");

            return NoContent();
        }
    }
}
