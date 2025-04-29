using Basket.API.Data.Entities;
using Microsoft.Extensions.Caching.Distributed;
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
            if (entity.ID <= 0)
            {
                entity.ID = await GetNextIdAsync();
            }

            await SaveBasketToRedisAsync(entity);

            await _redisCache.SetStringAsync(
                GetUserBasketKey(entity.UserId),
                entity.ID.ToString(),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );
        }

        private async Task SaveBasketToRedisAsync(Baskett basket)
        {
            var basketJson = JsonSerializer.Serialize(basket);

            await _redisCache.SetStringAsync(
                GetBasketKey(basket.ID),
                basketJson,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );

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
    }
}