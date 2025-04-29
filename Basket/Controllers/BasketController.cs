using Basket.API.Data.Entities;
using Basket.API.DTOS.BasketDTO.Basket;
using Basket.API.Services.BasketServices;
using Logging.Shared.Models.Logging.Shared.Models;
using Logging.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ILogPublisher _logPublisher;

        public BasketController(
            IBasketService basketService,
            ILogPublisher logPublisher)
        {
            _basketService = basketService;
            _logPublisher = logPublisher;
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

            return Created($"/api/basket/{basket.ID}", basket);
        }
    }
}