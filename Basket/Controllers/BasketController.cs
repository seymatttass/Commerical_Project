using Basket.API.Data.Entities;
using Basket.API.DTOS;
using Basket.API.DTOS.BasketDTO.Basket;
using Basket.API.Services.BasketServices;
using Basket.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Events.BasketEvents;
using MassTransit;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IBasketItemService _basketItemService;
        private readonly IPublishEndpoint _publishEndpoint;

    

        public BasketController(IBasketService basketService, IBasketItemService basketItemService, IPublishEndpoint publishEndpoint)
        {
            _basketService = basketService;
            _basketItemService = basketItemService;
            _publishEndpoint = publishEndpoint;
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
            // Güvenlik için toplam fiyatı hesapla
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
            // ProductId artık CreateBasketItemDTO içinde tanımlanmalı
            if (itemDto.ProductId <= 0)
            {
                return BadRequest("ProductId is required");
            }

            var basketItem = await _basketItemService.AddAsync(itemDto);

            // Sepet öğesi eklendiğinde event'i yayınla
            var productAddedEvent = new ProductAddedToBasketRequestEvent(Guid.NewGuid())
            {
                ProductId = basketItem.ProductId,
                Count = basketItem.Count,
                Price = basketItem.Price
            };

            await _publishEndpoint.Publish(productAddedEvent);

            return CreatedAtAction("GetBasketItem", "BasketItem", new { id = basketItem.ID }, basketItem);
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

        // PUT: api/Basket/items/5
        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateBasketItem(int id, UpdateBasketItemDTO itemDto)
        {
            if (id != itemDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var success = await _basketItemService.UpdateAsync(itemDto);

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

            // Burada ödeme işlemi ve sipariş oluşturma mantığı eklenebilir
            // Şimdilik sepeti siliyoruz
            var success = await _basketService.DeleteAsync(id);

            if (!success)
            {
                return StatusCode(500, "Failed to process checkout");
            }

            return Ok(new { message = "Checkout completed successfully" });
        }
    }
}