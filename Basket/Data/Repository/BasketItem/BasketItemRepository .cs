using Basket.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basket.API.Data.Repository
{
    public class BasketItemRepository : IBasketItemRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly string _basketItemPrefix = "basketitem:";
        private readonly string _basketPrefix = "basket:";

        public BasketItemRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        private string GetBasketItemKey(int id)
        {
            return $"{_basketItemPrefix}{id}";
        }

        private string GetBasketKey(int id)
        {
            return $"{_basketPrefix}{id}";
        }

        public async Task<IEnumerable<BasketItem>> GetByBasketIdAsync(int basketId)
        {
            // First, try to get the basket
            var basketJson = await _redisCache.GetStringAsync(GetBasketKey(basketId));

            if (string.IsNullOrEmpty(basketJson))
            {
                return new List<BasketItem>();
            }

            var basket = JsonSerializer.Deserialize<Baskett>(basketJson);
            return basket.BasketItems ?? new List<BasketItem>();
        }

        public async Task<BasketItem> GetByIdAsync(int itemId)
        {
            // With Redis approach, we need to find the basket containing this item
            // This is inefficient but maintains compatibility

            // Get all baskets (in a real implementation, you would improve this)
            var basketIdsJson = await _redisCache.GetStringAsync("basket:ids");
            if (string.IsNullOrEmpty(basketIdsJson))
            {
                return null;
            }

            var basketIds = JsonSerializer.Deserialize<List<int>>(basketIdsJson);

            foreach (var basketId in basketIds)
            {
                var basketJson = await _redisCache.GetStringAsync(GetBasketKey(basketId));
                if (!string.IsNullOrEmpty(basketJson))
                {
                    var basket = JsonSerializer.Deserialize<Baskett>(basketJson);
                    var item = basket.BasketItems?.FirstOrDefault(i => i.ID == itemId);

                    if (item != null)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public async Task AddAsync(BasketItem basketItem)
        {
            // Generate ID if not set
            if (basketItem.ID <= 0)
            {
                basketItem.ID = await GetNextItemIdAsync();
            }

            // Find the parent basket
            var basketJson = await _redisCache.GetStringAsync(GetBasketKey(basketItem.BasketId));

            if (string.IsNullOrEmpty(basketJson))
            {
                throw new KeyNotFoundException($"Basket with ID {basketItem.BasketId} not found");
            }

            var basket = JsonSerializer.Deserialize<Baskett>(basketJson);

            // Initialize basket items list if null
            if (basket.BasketItems == null)
            {
                basket.BasketItems = new List<BasketItem>();
            }

            // Add the new item
            basket.BasketItems.Add(basketItem);

            // Update the total price
            basket.TotalPrice += basketItem.Price * basketItem.Count;

            // Save the updated basket back to Redis
            await _redisCache.SetStringAsync(
                GetBasketKey(basket.ID),
                JsonSerializer.Serialize(basket),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );
        }

        public async Task<bool> RemoveAsync(int itemId)
        {
            // Find which basket contains this item
            var basketItem = await GetByIdAsync(itemId);

            if (basketItem == null)
            {
                return false;
            }

            var basketJson = await _redisCache.GetStringAsync(GetBasketKey(basketItem.BasketId));

            if (string.IsNullOrEmpty(basketJson))
            {
                return false;
            }

            var basket = JsonSerializer.Deserialize<Baskett>(basketJson);

            // Find the item to remove
            var itemToRemove = basket.BasketItems.FirstOrDefault(i => i.ID == itemId);

            if (itemToRemove == null)
            {
                return false;
            }

            // Remove the item
            basket.BasketItems.Remove(itemToRemove);

            // Update total price
            basket.TotalPrice -= itemToRemove.Price * itemToRemove.Count;

            // Save the updated basket
            await _redisCache.SetStringAsync(
                GetBasketKey(basket.ID),
                JsonSerializer.Serialize(basket),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );

            return true;
        }

        public async Task<bool> UpdateAsync(BasketItem basketItem)
        {
            // Find which basket contains this item
            var existingItem = await GetByIdAsync(basketItem.ID);

            if (existingItem == null)
            {
                return false;
            }

            var basketJson = await _redisCache.GetStringAsync(GetBasketKey(existingItem.BasketId));

            if (string.IsNullOrEmpty(basketJson))
            {
                return false;
            }

            var basket = JsonSerializer.Deserialize<Baskett>(basketJson);

            // Find the item to update
            var itemIndex = basket.BasketItems.FindIndex(i => i.ID == basketItem.ID);

            if (itemIndex == -1)
            {
                return false;
            }

            // Update total price (subtract old price and add new price)
            var oldItem = basket.BasketItems[itemIndex];
            basket.TotalPrice -= oldItem.Price * oldItem.Count;
            basket.TotalPrice += basketItem.Price * basketItem.Count;

            // Update the item
            basket.BasketItems[itemIndex] = basketItem;

            // Save the updated basket
            await _redisCache.SetStringAsync(
                GetBasketKey(basket.ID),
                JsonSerializer.Serialize(basket),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );

            return true;
        }

        // Helper methods for Redis implementation

        private async Task<int> GetNextItemIdAsync()
        {
            var nextIdKey = "basketitem:next_id";
            var nextIdStr = await _redisCache.GetStringAsync(nextIdKey);

            int nextId = 1;
            if (!string.IsNullOrEmpty(nextIdStr) && int.TryParse(nextIdStr, out int storedNextId))
            {
                nextId = storedNextId;
            }

            // Increment for next use
            await _redisCache.SetStringAsync(
                nextIdKey,
                (nextId + 1).ToString(),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365) }
            );

            return nextId;
        }
    }
}
