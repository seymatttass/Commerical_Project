using Basket.API.Data;
using Basket.API.Data.Entities;
using Basket.API.DTOS;
using Basket.API.DTOS.BasketDTO.Basket;
using Basket.API.Services.BasketServices;
using Basket.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IBasketItemService _basketItemService;

        public BasketController(IBasketService basketService, IBasketItemService basketItemService)
        {
            _basketService = basketService;
            _basketItemService = basketItemService;
        }

        // GET: api/Basket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Baskett>>> GetBaskets()
        {
            var baskets = await _basketService.GetAllAsync();
            return Ok(baskets);
        }

        // GET: api/Basket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Baskett>> GetBasket(int id)
        {
            var basket = await _basketService.GetByIdAsync(id);

            if (basket == null)
            {
                return NotFound();
            }

            return Ok(basket);
        }

        // GET: api/Basket/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Baskett>> GetBasketByUserId(int userId)
        {
            var baskets = await _basketService.GetAllAsync();
            var basket = baskets.FirstOrDefault(b => b.UserId == userId);

            if (basket == null)
            {
                return NotFound();
            }

            return Ok(basket);
        }

        // POST: api/Basket
        [HttpPost]
        public async Task<ActionResult<Baskett>> CreateBasket(CreateBasketDTO basketDto)
        {
            // Calculate total price for security
            decimal calculatedTotalPrice = 0;

            if (basketDto.BasketItems != null)
            {
                foreach (var item in basketDto.BasketItems)
                {
                    calculatedTotalPrice += item.Price * item.Count;
                }
            }

            var basket = await _basketService.AddAsync(basketDto);

            return CreatedAtAction(nameof(GetBasket), new { id = basket.ID }, basket);
        }

        // POST: api/Basket/items
        [HttpPost("items")]
        public async Task<ActionResult<BasketItem>> AddBasketItem(CreateBasketItemDTO itemDto)
        {
            if (itemDto.BasketId <= 0)
            {
                return BadRequest("BasketId is required when adding items separately");
            }

            var basket = await _basketService.GetByIdAsync(itemDto.BasketId);
            if (basket == null)
            {
                return NotFound("Basket not found");
            }

            var basketItem = await _basketItemService.AddAsync(itemDto);

            return CreatedAtAction("GetBasketItem", new { id = basketItem.ID }, basketItem);
        }

        // GET: api/Basket/items/5
        [HttpGet("items/{id}")]
        public async Task<ActionResult<BasketItem>> GetBasketItem(int id)
        {
            var basketItem = await _basketItemService.GetByIdAsync(id);

            if (basketItem == null)
            {
                return NotFound();
            }

            return Ok(basketItem);
        }

        // GET: api/Basket/{basketId}/items
        [HttpGet("{basketId}/items")]
        public async Task<ActionResult<IEnumerable<BasketItem>>> GetBasketItems(int basketId)
        {
            var basket = await _basketService.GetByIdAsync(basketId);

            if (basket == null)
            {
                return NotFound("Basket not found");
            }

            var basketItems = await _basketItemService.GetByBasketIdAsync(basketId);
            return Ok(basketItems);
        }

        // PUT: api/Basket/items/5
        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateBasketItem(int id, CreateBasketItemDTO itemDto)
        {
            var basketItem = await _basketItemService.GetByIdAsync(id);
            if (basketItem == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateBasketItemDTO
            {
                Id = id,
                BasketId = itemDto.BasketId,
                Price = itemDto.Price,
                Count = itemDto.Count
            };

            var success = await _basketItemService.UpdateAsync(updateDto);

            if (!success)
            {
                return BadRequest("Failed to update basket item");
            }

            return NoContent();
        }

        // DELETE: api/Basket/items/5
        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteBasketItem(int id)
        {
            var success = await _basketItemService.DeleteAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Basket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var success = await _basketService.DeleteAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Basket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBasket(int id, UpdateBasketDTO basketDto)
        {
            if (id != basketDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var success = await _basketService.UpdateAsync(basketDto);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Basket/checkout/5
        [HttpPost("checkout/{id}")]
        public async Task<IActionResult> CheckoutBasket(int id)
        {
            var basket = await _basketService.GetByIdAsync(id);

            if (basket == null)
            {
                return NotFound();
            }

            if (basket.BasketItems == null || !basket.BasketItems.Any())
            {
                return BadRequest("Cannot checkout an empty basket");
            }

            // Here you could implement checkout logic (payment processing, order creation)
            // For now, we simply delete the basket
            var success = await _basketService.DeleteAsync(id);

            if (!success)
            {
                return StatusCode(500, "Failed to process checkout");
            }

            return Ok(new { message = "Checkout completed successfully" });
        }
    }
}