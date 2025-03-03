using Microsoft.AspNetCore.Mvc;
using Basket.API.Data.Entities;
using Basket.API.Services.BasketServices;
using Basket.API.DTOS.BasketDTO.Basket;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;

        public BasketController(IBasketService basketService, IMapper mapper)
        {
            _basketService = basketService;
            _mapper = mapper;
        }

        // GET: api/basket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Baskett>>> GetAllBaskets()
        {
            var baskets = await _basketService.GetAllAsync();
            return Ok(baskets);
        }

        // GET: api/basket/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Baskett>> GetBasketById(int id)
        {
            var basket = await _basketService.GetByIdAsync(id);
            if (basket == null)
                return NotFound("Basket not found.");

            return Ok(basket);
        }

        // POST: api/basket
        [HttpPost]
        public async Task<ActionResult> CreateBasket([FromBody] CreateBasketDTO basketDTO)
        {
            var basket = await _basketService.AddAsync(basketDTO);
            return CreatedAtAction(nameof(GetBasketById), new { id = basket.ID }, basket);
        }

        // PUT: api/basket/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBasket(int id, [FromBody] UpdateBasketDTO basketDTO)
        {
            if (id != basketDTO.Id)
                return BadRequest("Basket ID mismatch.");

            var existingBasket = await _basketService.GetByIdAsync(id);
            if (existingBasket == null)
                return NotFound("Basket not found.");

            var result = await _basketService.UpdateAsync(basketDTO);
            if (!result) return BadRequest("Failed to update basket.");

            return NoContent();
        }

        // DELETE: api/basket/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var result = await _basketService.DeleteAsync(id);
            if (!result)
                return NotFound("Basket not found.");

            return NoContent();
        }
    }
}
