using Basket.API.Data.Entities;
using Basket.API.DTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Services
{
    public interface IBasketItemService
    {
        Task<IEnumerable<BasketItem>> GetByBasketIdAsync(int basketId);
        Task<BasketItem> GetByIdAsync(int itemId);
        Task<BasketItem> AddAsync(CreateBasketItemDTO createBasketItemDto);
        Task<bool> UpdateAsync(UpdateBasketItemDTO updateBasketItemDto);
        Task<bool> DeleteAsync(int itemId);
    }
}
