using Basket.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Data.Repository
{
    public interface IBasketItemRepository
    {
        Task<BasketItem> GetByIdAsync(int itemId);
        Task AddAsync(BasketItem basketItem);
        Task<bool> RemoveAsync(int itemId);
        Task<bool> UpdateAsync(BasketItem basketItem);
    }
}
