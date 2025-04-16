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
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IBasketItemService _basketItemService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketService basketService, IBasketItemService basketItemService, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger)
        {
            _basketService = basketService;
            _basketItemService = basketItemService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Baskett>>> GetBaskets()
        {
            _logger.LogInformation("Tüm sepetler alınıyor.");
            var baskets = await _basketService.GetAllAsync();
            _logger.LogInformation("Tüm sepetler başarıyla alındı.");
            return Ok(baskets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Baskett>> GetBasket(int id)
        {
            _logger.LogInformation("Sepet bilgisi alınıyor. SepetId: {id}", id);
            var basket = await _basketService.GetByIdAsync(id);

            if (basket == null)
            {
                _logger.LogWarning("Sepet bulunamadı. SepetId: {id}", id);
                return NotFound();
            }

            _logger.LogInformation("Sepet başarıyla alındı. SepetId: {id}", id);
            return Ok(basket);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Baskett>> GetBasketByUserId(int userId)
        {
            _logger.LogInformation("Kullanıcıya ait sepet bilgisi alınıyor. KullanıcıId: {userId}", userId);
            var baskets = await _basketService.GetAllAsync();
            var basket = baskets.FirstOrDefault(b => b.UserId == userId);

            if (basket == null)
            {
                _logger.LogWarning("Kullanıcıya ait sepet bulunamadı. KullanıcıId: {userId}", userId);
                return NotFound();
            }

            _logger.LogInformation("Kullanıcıya ait sepet başarıyla alındı. KullanıcıId: {userId}", userId);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<Baskett>> CreateBasket(CreateBasketDTO basketDto)
        {
            decimal calculatedTotalPrice = 0;

            _logger.LogInformation("Yeni sepet oluşturuluyor.");
            foreach (var item in basketDto.BasketItems)
            {
                calculatedTotalPrice += item.Price * item.Count;
            }

            var basket = await _basketService.AddAsync(basketDto);
            _logger.LogInformation("Sepet başarıyla oluşturuldu. SepetId: {id}", basket.ID);
            return CreatedAtAction(nameof(GetBasket), new { id = basket.ID }, basket);
        }

        [HttpPost("items")]
        public async Task<ActionResult<BasketItem>> AddBasketItem(CreateBasketItemDTO itemDto)
        {
            _logger.LogInformation("Sepet öğesi ekleniyor. ÜrünId: {productId}", itemDto.ProductId);

            if (itemDto.ProductId <= 0)
            {
                _logger.LogWarning("Geçersiz ürünId: {productId}", itemDto.ProductId);
                return BadRequest("ProductId is required");
            }

            var basketItem = await _basketItemService.AddAsync(itemDto);

            var productAddedEvent = new ProductAddedToBasketRequestEvent()
            {
                ProductId = basketItem.ProductId,
                Count = basketItem.Count,
                Price = basketItem.Price
            };

            await _publishEndpoint.Publish(productAddedEvent);
            _logger.LogInformation("Sepet öğesi başarıyla eklendi. ÜrünId: {productId}", itemDto.ProductId);

            return CreatedAtAction("GetBasketItem", "BasketItem", new { id = basketItem.ID }, basketItem);
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<BasketItem>> GetBasketItem(int id)
        {
            _logger.LogInformation("Sepet öğesi alınıyor. ÖğesiId: {id}", id);
            var basketItem = await _basketItemService.GetByIdAsync(id);

            if (basketItem == null)
            {
                _logger.LogWarning("Sepet öğesi bulunamadı. ÖğesiId: {id}", id);
                return NotFound();
            }

            _logger.LogInformation("Sepet öğesi başarıyla alındı. ÖğesiId: {id}", id);
            return Ok(basketItem);
        }

        [HttpPost("checkout/{id}")]
        public async Task<IActionResult> CheckoutBasket(int id)
        {
            _logger.LogInformation("Sepet ödeme işlemi başlatılıyor. SepetId: {id}", id);
            var basket = await _basketService.GetByIdAsync(id);

            if (basket == null)
            {
                _logger.LogWarning("Sepet bulunamadı. SepetId: {id}", id);
                return NotFound();
            }

            if (basket.BasketItems == null || !basket.BasketItems.Any())
            {
                _logger.LogWarning("Sepet boş, ödeme işlemi yapılmaz. SepetId: {id}", id);
                return BadRequest("Cannot checkout an empty basket");
            }

            var success = await _basketService.DeleteAsync(id);

            if (!success)
            {
                _logger.LogError("Sepet ödeme işlemi başarısız oldu. SepetId: {id}", id);
                return StatusCode(500, "Failed to process checkout");
            }

            _logger.LogInformation("Sepet ödeme işlemi başarıyla tamamlandı. SepetId: {id}", id);
            return Ok(new { message = "Checkout completed successfully" });
        }
    }
}
