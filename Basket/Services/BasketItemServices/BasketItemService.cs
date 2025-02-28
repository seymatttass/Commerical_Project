using AutoMapper;
using Basket.API.Data.Entities;
using Basket.API.Data.Repository;
using Basket.API.DTOS;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Services
{
    public class BasketItemService : IBasketItemService
    {
        private readonly IBasketItemRepository _basketItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketItemService> _logger;

        public BasketItemService(
            IBasketItemRepository basketItemRepository,
            IMapper mapper,
            ILogger<BasketItemService> logger)
        {
            _basketItemRepository = basketItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BasketItem>> GetByBasketIdAsync(int basketId)
        {
            try
            {
                return await _basketItemRepository.GetByBasketIdAsync(basketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting items for basket {basketId}");
                throw;
            }
        }

        public async Task<BasketItem> GetByIdAsync(int itemId)
        {
            try
            {
                return await _basketItemRepository.GetByIdAsync(itemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting basket item {itemId}");
                throw;
            }
        }

        public async Task<BasketItem> AddAsync(CreateBasketItemDTO createBasketItemDto)
        {
            try
            {
                var basketItem = _mapper.Map<BasketItem>(createBasketItemDto);
                await _basketItemRepository.AddAsync(basketItem);
                return basketItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding basket item");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UpdateBasketItemDTO updateBasketItemDto)
        {
            try
            {
                var basketItem = _mapper.Map<BasketItem>(updateBasketItemDto);
                return await _basketItemRepository.UpdateAsync(basketItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating basket item {updateBasketItemDto.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int itemId)
        {
            try
            {
                return await _basketItemRepository.RemoveAsync(itemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting basket item {itemId}");
                throw;
            }
        }
    }
}
