using Basket.API.Data.Entities;
using Basket.API.DTOS.BasketDTO.Basket;

namespace Basket.API.Services.BasketServices
{
    public interface IBasketService
    {
        Task<Baskett> AddAsync(CreateBasketDTO createBasketDto);
    }
}