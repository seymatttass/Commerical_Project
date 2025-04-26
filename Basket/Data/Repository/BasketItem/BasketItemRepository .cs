﻿using Basket.API.Data.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Basket.API.DTOS.BasketDTO;

namespace Basket.API.Data.Repository
{
    public class BasketItemRepository : IBasketItemRepository
    {
        private readonly IDistributedCache _redisCache; //BasketItemRepository sınıfının içinde redis ile iletşim kurmak için IDistributedCache nesnesi  
        private readonly string _basketItemPrefix = "basketitem:"; 
        public BasketItemRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<BasketItem> AddToBasketAsync(int basketId, AddToBasketItemDTO dto)
        {
            var basketItem = new BasketItem
            {
                ProductId = dto.ProductId,
                Count = dto.Count,
                Price = dto.Price,
                BasketID = basketId 
            };
            await AddAsync(basketItem);
            return basketItem;
        }
        
        private string GetBasketItemKey(int id)
        {
            return $"{_basketItemPrefix}{id}";
        }

      
        public async Task<BasketItem> GetByIdAsync(int itemId)
        {
            var key = GetBasketItemKey(itemId);
            var itemJson = await _redisCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(itemJson))
            {
                return null;
            }
            return JsonSerializer.Deserialize<BasketItem>(itemJson);
        }


        public async Task AddAsync(BasketItem basketItem)
        {
            if (basketItem.ID <= 0)
            {
                basketItem.ID = await GetNextItemIdAsync();
            }
            var key = GetBasketItemKey(basketItem.ID);
            await _redisCache.SetStringAsync(
                key,
                JsonSerializer.Serialize(basketItem),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );
        }

        public async Task<bool> RemoveAsync(int itemId)
        {
            var existingItem = await GetByIdAsync(itemId);
            if (existingItem == null)
            {
                return false;
            }
            var key = GetBasketItemKey(itemId);
            await _redisCache.RemoveAsync(key);
            return true;
        }

        public async Task<bool> UpdateAsync(BasketItem basketItem)
        {
            var existingItem = await GetByIdAsync(basketItem.ID);
            if (existingItem == null)
            {
                return false;
            }
            var key = GetBasketItemKey(basketItem.ID);
            await _redisCache.SetStringAsync(
                key,
                JsonSerializer.Serialize(basketItem),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );
            return true;
        }

        private async Task<int> GetNextItemIdAsync()
        {
            var nextIdKey = "basketitem:next_id";
            var nextIdStr = await _redisCache.GetStringAsync(nextIdKey);
            int nextId = 1;
            if (!string.IsNullOrEmpty(nextIdStr) && int.TryParse(nextIdStr, out int storedNextId))
            {
                nextId = storedNextId;
            }
            await _redisCache.SetStringAsync(
                nextIdKey,
                (nextId + 1).ToString(),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );
            return nextId;
        }
    }
}