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
                        var productAddedEvent = new ProductAddedToBasketRequestEvent(Guid.NewGuid())
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

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _basketRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting basket {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Baskett>> GetAllAsync()
        {
            try
            {
                return await _basketRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all baskets");
                throw;
            }
        }

        public async Task<Baskett> GetByIdAsync(int id)
        {
            try
            {
                return await _basketRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting basket {id}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UpdateBasketDTO updateBasketDto)
        {
            try
            {
                var basket = _mapper.Map<Baskett>(updateBasketDto);

                return await _basketRepository.UpdateAsync(basket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating basket {updateBasketDto.Id}");
                throw;
            }
        }
    }
}
