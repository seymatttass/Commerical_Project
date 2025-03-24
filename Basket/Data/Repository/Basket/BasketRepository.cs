using Basket.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;
using System.Text.Json;

namespace Basket.API.Data.Repository.Basket
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly string _basketPrefix = "basket:";

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        private string GetBasketKey(int id)
        {
            return $"{_basketPrefix}{id}";
        }

        private string GetUserBasketKey(int userId)
        {
            return $"user:{userId}:basket";
        }

        public async Task AddAsync(Baskett entity)
        {
            // Generate ID if not already set
            if (entity.ID <= 0)
            {
                entity.ID = await GetNextIdAsync();
            }

            // Save the basket
            await SaveBasketToRedisAsync(entity);

            // Create index for user-basket lookup
            await _redisCache.SetStringAsync(
                GetUserBasketKey(entity.UserId),
                entity.ID.ToString(),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );
        }

        public async Task AddRangeAsync(IEnumerable<Baskett> entities)
        {
            foreach (var entity in entities)
            {
                await AddAsync(entity);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var basketJson = await _redisCache.GetStringAsync(GetBasketKey(id));
            return !string.IsNullOrEmpty(basketJson);
        }

        public async Task<IEnumerable<Baskett>> FindAsync(System.Linq.Expressions.Expression<Func<Baskett, bool>> predicate)
        {
            // This is inefficient with Redis, but required for compatibility
            // In a real implementation, you'd want to redesign this based on your query patterns
            var allBaskets = await GetAllAsync();
            return allBaskets.AsQueryable().Where(predicate).ToList();
        }

        public async Task<IEnumerable<Baskett>> GetAllAsync()
        {
            // Warning: This implementation is for compatibility but not efficient in Redis
            // It requires scanning all keys with the basket prefix
            var result = new List<Baskett>();

            // This is a simplified example - in production you would use Redis SCAN
            // For now, we'll assume you have a way to get all basket IDs
            // This could be implemented by maintaining a separate set in Redis
            var basketIds = await GetAllBasketIdsAsync();

            foreach (var id in basketIds)
            {
                var basket = await GetByIdAsync(id);
                if (basket != null)
                {
                    result.Add(basket);
                }
            }

            return result;
        }

        public async Task<Baskett> GetByIdAsync(int id)
        {
            var basketJson = await _redisCache.GetStringAsync(GetBasketKey(id));

            if (string.IsNullOrEmpty(basketJson))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Baskett>(basketJson);
        }

        public async Task<Baskett> GetByUserIdAsync(int userId)
        {
            var basketIdStr = await _redisCache.GetStringAsync(GetUserBasketKey(userId));

            if (string.IsNullOrEmpty(basketIdStr) || !int.TryParse(basketIdStr, out int basketId))
            {
                return null;
            }

            return await GetByIdAsync(basketId);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var basket = await GetByIdAsync(id);
            if (basket == null)
            {
                return false;
            }

            // Remove the user-basket index
            await _redisCache.RemoveAsync(GetUserBasketKey(basket.UserId));

            // Remove each basket item
            foreach (var item in basket.BasketItems)
            {
                await _redisCache.RemoveAsync($"basketitem:{item.ID}");
            }

            // Remove the basket
            await _redisCache.RemoveAsync(GetBasketKey(id));

            // Update basket ID list
            await RemoveBasketIdAsync(id);

            return true;
        }

        public async Task RemoveRangeAsync(IEnumerable<Baskett> entities)
        {
            foreach (var entity in entities)
            {
                await RemoveAsync(entity.ID);
            }
        }

        public async Task<bool> UpdateAsync(Baskett entity)
        {
            // Check if entity exists
            if (!await ExistsAsync(entity.ID))
            {
                return false;
            }

            // Save the updated basket
            await SaveBasketToRedisAsync(entity);

            // Update user-basket index if user ID changed
            var existingBasket = await GetByIdAsync(entity.ID);
            if (existingBasket.UserId != entity.UserId)
            {
                // Remove old mapping
                await _redisCache.RemoveAsync(GetUserBasketKey(existingBasket.UserId));

                // Add new mapping
                await _redisCache.SetStringAsync(
                    GetUserBasketKey(entity.UserId),
                    entity.ID.ToString(),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
                );
            }

            return true;
        }

        // Helper methods for Redis implementation

        private async Task SaveBasketToRedisAsync(Baskett basket)
        {
            var basketJson = JsonSerializer.Serialize(basket);

            await _redisCache.SetStringAsync(
                GetBasketKey(basket.ID),
                basketJson,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );

            // Update basket ID list
            await AddBasketIdAsync(basket.ID);
        }

        private async Task<int> GetNextIdAsync()
        {
            var nextIdKey = "basket:next_id";
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

        private async Task<List<int>> GetAllBasketIdsAsync()
        {
            var idsJson = await _redisCache.GetStringAsync("basket:ids");
            if (string.IsNullOrEmpty(idsJson))
            {
                return new List<int>();
            }

            return JsonSerializer.Deserialize<List<int>>(idsJson);
        }

        private async Task AddBasketIdAsync(int id)
        {
            var ids = await GetAllBasketIdsAsync();

            if (!ids.Contains(id))
            {
                ids.Add(id);
                await _redisCache.SetStringAsync(
                    "basket:ids",
                    JsonSerializer.Serialize(ids),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365) }
                );
            }
        }

        private async Task RemoveBasketIdAsync(int id)
        {
            var ids = await GetAllBasketIdsAsync();

            if (ids.Contains(id))
            {
                ids.Remove(id);
                await _redisCache.SetStringAsync(
                    "basket:ids",
                    JsonSerializer.Serialize(ids),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365) }
                );
            }
        }
    }
}