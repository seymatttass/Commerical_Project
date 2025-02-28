using Basket.API.Data.Entities;
using Basket.API.DTOS;

namespace Basket.API.Services
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
