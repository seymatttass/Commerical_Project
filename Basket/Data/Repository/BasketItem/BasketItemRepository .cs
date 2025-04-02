using Basket.API.Data.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Basket.API.DTOS.BasketDTO;

namespace Basket.API.Data.Repository
{
    /// <summary>
    /// Redis önbellek kullanarak sepet öğelerini yöneten depo sınıfı.
    /// IBasketItemRepository arayüzünü uygular ve sepet öğesi işlemlerini Redis'te gerçekleştirir.
    /// </summary>
    public class BasketItemRepository : IBasketItemRepository
    {
        private readonly IDistributedCache _redisCache; // Redis önbellek bağlantısı
        private readonly string _basketItemPrefix = "basketitem:"; // Sepet öğesi anahtarları için önek

        /// <summary>
        /// BasketItemRepository sınıfı için yapıcı metod
        /// </summary>
        /// <param name="redisCache">DI ile enjekte edilen Redis önbellek nesnesi</param>
        public BasketItemRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        /// <summary>
        /// Bir sepete yeni bir öğe ekler
        /// </summary>
        /// <param name="basketId">Öğenin ekleneceği sepet ID'si</param>
        /// <param name="dto">Eklenecek öğe bilgilerini içeren DTO</param>
        /// <returns>Eklenen sepet öğesi</returns>
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
        
        /// <summary>
        /// Sepet öğesi ID'sine göre Redis anahtarını oluşturur
        /// </summary>
        /// <param name="id">Sepet öğesi ID'si</param>
        /// <returns>Redis'te kullanılacak tam anahtar adı</returns>
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



        /// <summary>
        /// Yeni bir sepet öğesi ekler
        /// </summary>
        public async Task AddAsync(BasketItem basketItem)
        {
            // Eğer ID atanmamışsa yeni bir ID ata
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

        /// <summary>
        /// Sepet öğesini ID'ye göre siler
        /// </summary>
        /// <param name="itemId">Silinecek sepet öğesi ID'si</param>
        /// <returns>Silme başarılı ise true, yoksa false</returns>
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

        /// <summary>
        /// Sepet öğesini günceller
        /// </summary>
        /// <param name="basketItem">Güncellenecek sepet öğesi nesnesi</param>
        /// <returns>Güncelleme başarılı ise true, yoksa false</returns>
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

        /// <summary>
        /// Bir sonraki sepet öğesi ID'sini alır
        /// </summary>
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