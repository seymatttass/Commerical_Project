using AutoMapper;
using Basket.API.Data.Entities;
using Basket.API.Data.Repository.Basket;
using Basket.API.DTOS.BasketDTO.Basket;
using Basket.API.Services.BasketServices;
using MassTransit;
using Shared.Events.BasketEvents;

namespace Basket.API.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketService> _logger;

        public BasketService(
            IBasketRepository basketRepository,
            IMapper mapper,
            ILogger<BasketService> logger,
            IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Baskett> AddAsync(CreateBasketDTO createBasketDto)
        {
            try
            {
                var basket = _mapper.Map<Baskett>(createBasketDto);
                await _basketRepository.AddAsync(basket);

                // Sepet oluşturulduktan sonra, her bir sepet öğesi için ProductAddedToBasketRequestEvent gönder
                if (basket.BasketItems != null && basket.BasketItems.Any())
                {
                    foreach (var item in basket.BasketItems)
                    {
                        var productAddedEvent = new ProductAddedToBasketRequestEvent()
                        {
                            ProductId = item.ProductId,
                            Count = item.Count,
                            UserId = basket.UserId,
                            Price = item.Price
                        };
                        await _publishEndpoint.Publish(productAddedEvent);
                    }
                }

                return basket;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating basket");
                throw;
            }
        }
    }
}