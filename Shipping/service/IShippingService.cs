using Shipping.API.DTOS.Shipping;

namespace Shipping.API.service
{
    public interface IShippingService
    {

        Task<IEnumerable<Data.Entities.Shipping>> GetAllAsync();
        Task<Data.Entities.Shipping> GetByIdAsync(int id);
        Task<Data.Entities.Shipping> AddAsync(CreateShippingDTO createShippingDto);
        Task<bool> UpdateAsync(UpdateShippingDTO updateShippingDto);
        Task<bool> DeleteAsync(int id);
    }
}
