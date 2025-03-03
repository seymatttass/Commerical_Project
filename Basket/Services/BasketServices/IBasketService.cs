using Basket.API.Data.Entities;
using Basket.API.DTOS.BasketDTO.Basket;

namespace Basket.API.Services.BasketServices
{
    public interface IBasketService
    {
        Task<IEnumerable<Baskett>> GetAllAsync();
        Task<Baskett> GetByIdAsync(int id);
        Task<Baskett> AddAsync(CreateBasketDTO createBasketDto);
        Task<bool> UpdateAsync(UpdateBasketDTO updateBasketDto);
        Task<bool> DeleteAsync(int id);
    }
}
