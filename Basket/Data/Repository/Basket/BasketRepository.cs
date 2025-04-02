using Basket.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;
using System.Text.Json;

namespace Basket.API.Data.Repository.Basket
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache; // Redis önbellek bağlantısı
        private readonly string _basketPrefix = "basket:"; // Sepet anahtarları için önek

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        /// <summary>
        /// Sepet ID'sine göre Redis anahtarını oluşturur
        /// </summary>
        /// <param name="id">Sepet ID'si</param>
        /// <returns>Redis'te kullanılacak tam anahtar adı</returns>
        private string GetBasketKey(int id)
        {
            return $"{_basketPrefix}{id}";
        }

        /// <summary>
        /// Kullanıcı ID'sine göre sepet indeks anahtarını oluşturur
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Kullanıcı-sepet eşleşmesi için kullanılan Redis anahtarı</returns>
        private string GetUserBasketKey(int userId)
        {
            return $"user:{userId}:basket";
        }

        /// <summary>
        /// Yeni bir sepet ekler
        /// </summary>
        /// <param name="entity">Eklenecek sepet nesnesi</param>
        public async Task AddAsync(Baskett entity)
        {
            // Eğer ID atanmamışsa yeni bir ID ata
            if (entity.ID <= 0)
            {
                entity.ID = await GetNextIdAsync();
            }

            await SaveBasketToRedisAsync(entity);

            // Kullanıcı-sepet ilişkisi için indeks oluştur
            await _redisCache.SetStringAsync(
                GetUserBasketKey(entity.UserId),
                entity.ID.ToString(),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );
        }


        /// <summary>
        /// Belirli bir ID'ye sahip sepetin var olup olmadığını kontrol eder
        /// </summary>
        /// <param name="id">Kontrol edilecek sepet ID'si</param>
        /// <returns>Sepet varsa true, yoksa false</returns>
        public async Task<bool> ExistsAsync(int id)
        {
            var basketJson = await _redisCache.GetStringAsync(GetBasketKey(id));
            return !string.IsNullOrEmpty(basketJson);
        }

        /// <summary>
        /// Verilen koşullara uyan sepetleri bulur
        /// </summary>
        /// <param name="predicate">Sorgulama için filtre koşulu</param>
        /// <returns>Koşullara uyan sepetlerin koleksiyonu</returns>
        public async Task<IEnumerable<Baskett>> FindAsync(Expression<Func<Baskett, bool>> predicate)
        {
            // Redis ile verimli olmayan ama uyumluluk için gerekli olan bir yöntem
            // Gerçek uygulamada, sorgu modellerinize göre yeniden tasarlamanız önerilir
            var allBaskets = await GetAllAsync();
            return allBaskets.AsQueryable().Where(predicate).ToList();
        }

        /// <summary>
        /// Tüm sepetleri getirir
        /// </summary>
        /// <returns>Tüm sepetlerin koleksiyonu</returns>
        public async Task<IEnumerable<Baskett>> GetAllAsync()
        {
            var result = new List<Baskett>();

            // Bu basitleştirilmiş bir örnek - üretimde Redis SCAN kullanmalısınız
            // Şimdilik tüm sepet ID'lerini alabileceğimiz bir yöntem olduğunu varsayıyoruz
            // Bu, Redis'te ayrı bir küme tutarak uygulanabilir
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

        /// <summary>
        /// ID'ye göre sepeti getirir
        /// </summary>
        /// <param name="id">Sepet ID'si</param>
        /// <returns>Bulunan sepet nesnesi veya null</returns>
        public async Task<Baskett> GetByIdAsync(int id)
        {
            var basketJson = await _redisCache.GetStringAsync(GetBasketKey(id));

            if (string.IsNullOrEmpty(basketJson))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Baskett>(basketJson);
        }

        /// <summary>
        /// Kullanıcı ID'sine göre sepeti getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Kullanıcının sepeti veya null</returns>
        public async Task<Baskett> GetByUserIdAsync(int userId)
        {
            var basketIdStr = await _redisCache.GetStringAsync(GetUserBasketKey(userId));

            if (string.IsNullOrEmpty(basketIdStr) || !int.TryParse(basketIdStr, out int basketId))
            {
                return null;
            }

            return await GetByIdAsync(basketId);
        }

        /// <summary>
        /// Sepeti ID'ye göre siler
        /// </summary>
        /// <param name="id">Silinecek sepet ID'si</param>
        /// <returns>Silme başarılı ise true, yoksa false</returns>
        public async Task<bool> RemoveAsync(int id)
        {
            var basket = await GetByIdAsync(id);
            if (basket == null)
            {
                return false;
            }

            // Kullanıcı-sepet indeksini kaldır
            await _redisCache.RemoveAsync(GetUserBasketKey(basket.UserId));

            // Her sepet öğesini kaldır
            foreach (var item in basket.BasketItems)
            {
                await _redisCache.RemoveAsync($"basketitem:{item.ID}");
            }

            // Sepeti kaldır
            await _redisCache.RemoveAsync(GetBasketKey(id));

            // Sepet ID listesini güncelle
            await RemoveBasketIdAsync(id);

            return true;
        }

        /// <summary>
        /// Birden fazla sepeti toplu olarak siler
        /// </summary>
        /// <param name="entities">Silinecek sepet nesneleri koleksiyonu</param>
        public async Task RemoveRangeAsync(IEnumerable<Baskett> entities)
        {
            foreach (var entity in entities)
            {
                await RemoveAsync(entity.ID);
            }
        }

        /// <summary>
        /// Sepeti günceller
        /// </summary>
        /// <param name="entity">Güncellenecek sepet nesnesi</param>
        /// <returns>Güncelleme başarılı ise true, yoksa false</returns>
        public async Task<bool> UpdateAsync(Baskett entity)
        {
            // Nesnenin var olup olmadığını kontrol et
            if (!await ExistsAsync(entity.ID))
            {
                return false;
            }

            // Güncellenmiş sepeti kaydet
            await SaveBasketToRedisAsync(entity);

            // Kullanıcı ID değiştiyse kullanıcı-sepet indeksini güncelle
            var existingBasket = await GetByIdAsync(entity.ID);
            if (existingBasket.UserId != entity.UserId)
            {
                // Eski eşleşmeyi kaldır
                await _redisCache.RemoveAsync(GetUserBasketKey(existingBasket.UserId));

                // Yeni eşleşme ekle
                await _redisCache.SetStringAsync(
                    GetUserBasketKey(entity.UserId),
                    entity.ID.ToString(),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
                );
            }

            return true;
        }

        // Redis uygulaması için yardımcı metodlar

        /// <summary>
        /// Sepeti Redis'e kaydeder
        /// </summary>
        /// <param name="basket">Kaydedilecek sepet nesnesi</param>
        private async Task SaveBasketToRedisAsync(Baskett basket)
        {
            var basketJson = JsonSerializer.Serialize(basket);

            await _redisCache.SetStringAsync(
                GetBasketKey(basket.ID),
                basketJson,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) }
            );

            // Sepet ID listesini güncelle
            await AddBasketIdAsync(basket.ID);
        }

        /// <summary>
        /// Bir sonraki sepet ID'sini alır
        /// </summary>
        /// <returns>Yeni sepet ID'si</returns>
        private async Task<int> GetNextIdAsync()
        {
            var nextIdKey = "basket:next_id";
            var nextIdStr = await _redisCache.GetStringAsync(nextIdKey);

            int nextId = 1;
            if (!string.IsNullOrEmpty(nextIdStr) && int.TryParse(nextIdStr, out int storedNextId))
            {
                nextId = storedNextId;
            }

            // Bir sonraki kullanım için artır
            await _redisCache.SetStringAsync(
                nextIdKey,
                (nextId + 1).ToString(),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365) }
            );

            return nextId;
        }

        /// <summary>
        /// Tüm sepet ID'lerini alır
        /// </summary>
        /// <returns>Sepet ID'leri listesi</returns>
        private async Task<List<int>> GetAllBasketIdsAsync()
        {
            var idsJson = await _redisCache.GetStringAsync("basket:ids");
            if (string.IsNullOrEmpty(idsJson))
            {
                return new List<int>();
            }

            return JsonSerializer.Deserialize<List<int>>(idsJson);
        }

        /// <summary>
        /// Sepet ID'sini global listeye ekler
        /// </summary>
        /// <param name="id">Eklenecek sepet ID'si</param>
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

        /// <summary>
        /// Sepet ID'sini global listeden kaldırır
        /// </summary>
        /// <param name="id">Kaldırılacak sepet ID'si</param>
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