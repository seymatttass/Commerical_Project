using Basket.API.Data.Entities;

namespace Basket.API.Data.Repository.Basket
{
    public interface IBasketRepository
    {
        Task AddAsync(Baskett entity);
    }
}