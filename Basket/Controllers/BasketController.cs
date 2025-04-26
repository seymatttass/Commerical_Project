using Basket.API.Data.Entities;
using Basket.API.DTOS;
using Basket.API.DTOS.BasketDTO.Basket;
using Basket.API.Services.BasketServices;
using Basket.API.Services;
using Logging.Shared.Models;
using Logging.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Events.BasketEvents;
using MassTransit;
using Logging.Shared.Models.Logging.Shared.Models;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService; //Sepet CRUD işlemleri.
        private readonly IBasketItemService _basketItemService; //Sepet öğeleri (ürünler) ile ilgili işlemler.
        private readonly IPublishEndpoint _publishEndpoint; //Sepet öğeleri (ürünler) ile ilgili işlemler.
        private readonly ILogPublisher _logPublisher; //Log kayıtlarını merkezi log servislerine (örneğin Elasticsearch) göndermek için kullanılır.

        public BasketController(
            IBasketService basketService,
            IBasketItemService basketItemService,
            IPublishEndpoint publishEndpoint,
            ILogPublisher logPublisher)
        {
            _basketService = basketService;
            _basketItemService = basketItemService;
            _publishEndpoint = publishEndpoint;
            _logPublisher = logPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Baskett>>> GetBaskets()
        {
            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = "Tüm sepetler alınıyor."
            });

            var baskets = await _basketService.GetAllAsync();

            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = "Tüm sepetler başarıyla alındı."
            });

            return Ok(baskets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Baskett>> GetBasket(int id)
        {
            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Sepet bilgisi alınıyor. SepetId: {id}"
            });

            var basket = await _basketService.GetByIdAsync(id);

            if (basket == null)
            {
                await _logPublisher.PublishLogAsync(new LogMessage
                {
                    Service = "Basket.API",
                    Level = "Warning",
                    Message = $"Sepet bulunamadı. SepetId: {id}"
                });

                return NotFound();
            }

            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Sepet başarıyla alındı. SepetId: {id}"
            });

            return Ok(basket);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Baskett>> GetBasketByUserId(int userId)
        {
            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Kullanıcıya ait sepet bilgisi alınıyor. KullanıcıId: {userId}"
            });

            var baskets = await _basketService.GetAllAsync();
            var basket = baskets.FirstOrDefault(b => b.UserId == userId);

            if (basket == null)
            {
                await _logPublisher.PublishLogAsync(new LogMessage
                {
                    Service = "Basket.API",
                    Level = "Warning",
                    Message = $"Kullanıcıya ait sepet bulunamadı. KullanıcıId: {userId}"
                });

                return NotFound();
            }

            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Kullanıcıya ait sepet başarıyla alındı. KullanıcıId: {userId}"
            });

            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<Baskett>> CreateBasket(CreateBasketDTO basketDto)
        {
            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = "Yeni sepet oluşturuluyor."
            });

            var basket = await _basketService.AddAsync(basketDto);

            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Sepet başarıyla oluşturuldu. SepetId: {basket.ID}"
            });

            return CreatedAtAction(nameof(GetBasket), new { id = basket.ID }, basket); // "Yeni sepet başarıyla oluşturuldu, işte ona ulaşabileceğin link ve tüm bilgiler."
        }

        [HttpPost("items")]
        public async Task<ActionResult<BasketItem>> AddBasketItem(CreateBasketItemDTO itemDto)
        {
            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Sepet öğesi ekleniyor. ÜrünId: {itemDto.ProductId}"
            });

            if (itemDto.ProductId <= 0)
            {
                await _logPublisher.PublishLogAsync(new LogMessage
                {
                    Service = "Basket.API",
                    Level = "Warning",
                    Message = $"Geçersiz ürünId: {itemDto.ProductId}"
                });

                return BadRequest("ProductId is required"); //istemciden (frontend, Postman, Swagger vs.) gelen hatalı veya eksik veriyi belirtmek için kullanılan bir HTTP 400 durum kodudur.
            }

            var basketItem = await _basketItemService.AddAsync(itemDto);

            var productAddedEvent = new ProductAddedToBasketRequestEvent
            {
                ProductId = basketItem.ProductId,
                Count = basketItem.Count,
                Price = basketItem.Price
            };

            await _publishEndpoint.Publish(productAddedEvent);

            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Sepet öğesi başarıyla eklendi. ÜrünId: {itemDto.ProductId}"
            });

            return CreatedAtAction("GetBasketItem", "BasketItem", new { id = basketItem.ID }, basketItem);
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<BasketItem>> GetBasketItem(int id)
        {
            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Sepet öğesi alınıyor. ÖğesiId: {id}"
            });

            var basketItem = await _basketItemService.GetByIdAsync(id);

            if (basketItem == null)
            {
                await _logPublisher.PublishLogAsync(new LogMessage
                {
                    Service = "Basket.API",
                    Level = "Warning",
                    Message = $"Sepet öğesi bulunamadı. ÖğesiId: {id}"
                });

                return NotFound();
            }

            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Sepet öğesi başarıyla alındı. ÖğesiId: {id}"
            });

            return Ok(basketItem);
        }

        [HttpPost("checkout/{id}")]
        public async Task<IActionResult> CheckoutBasket(int id)
        {
            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Sepet ödeme işlemi başlatılıyor. SepetId: {id}"
            });

            var basket = await _basketService.GetByIdAsync(id);

            if (basket == null)
            {
                await _logPublisher.PublishLogAsync(new LogMessage
                {
                    Service = "Basket.API",
                    Level = "Warning",
                    Message = $"Sepet bulunamadı. SepetId: {id}"
                });

                return NotFound();
            }

            if (basket.BasketItems == null || !basket.BasketItems.Any())
            {
                await _logPublisher.PublishLogAsync(new LogMessage
                {
                    Service = "Basket.API",
                    Level = "Warning",
                    Message = $"Sepet boş, ödeme yapılamaz. SepetId: {id}"
                });

                return BadRequest("Cannot checkout an empty basket");
            }

            var success = await _basketService.DeleteAsync(id);

            if (!success)
            {
                await _logPublisher.PublishLogAsync(new LogMessage
                {
                    Service = "Basket.API",
                    Level = "Error",
                    Message = $"Sepet ödeme işlemi başarısız oldu. SepetId: {id}"
                });

                return StatusCode(500, "Failed to process checkout");
            }

            await _logPublisher.PublishLogAsync(new LogMessage
            {
                Service = "Basket.API",
                Level = "Information",
                Message = $"Sepet ödeme işlemi başarıyla tamamlandı. SepetId: {id}"
            });

            return Ok(new { message = "Checkout completed successfully" });
        }
    }
}
