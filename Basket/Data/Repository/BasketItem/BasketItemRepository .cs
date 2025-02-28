using Basket.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Data.Repository
{
    public class BasketItemRepository : IBasketItemRepository
    {
        private readonly BasketDbContext _dbContext;

        public BasketItemRepository(BasketDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BasketItem>> GetByBasketIdAsync(int basketId)
        {
            return await _dbContext.BasketItems
                .Where(bi => bi.BasketId == basketId)
                .ToListAsync();
        }

        public async Task<BasketItem> GetByIdAsync(int itemId)
        {
            return await _dbContext.BasketItems.FirstOrDefaultAsync(bi => bi.ID == itemId);
        }

        public async Task AddAsync(BasketItem basketItem)
        {
            await _dbContext.BasketItems.AddAsync(basketItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(int itemId)
        {
            var item = await GetByIdAsync(itemId);
            if (item == null)
                return false;

            _dbContext.BasketItems.Remove(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(BasketItem basketItem)
        {
            var update = await GetByIdAsync(basketItem.ID);
            if (update == null)
                return false;

            _dbContext.Entry(update).CurrentValues.SetValues(basketItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
