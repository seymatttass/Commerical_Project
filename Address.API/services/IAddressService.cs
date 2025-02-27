using Address.API.Data.Entities;
using Address.API.DTOS.Address;

namespace Address.API.services
{
    public interface IAddressService
    {


        Task<IEnumerable<Addres>> GetAllAsync();
        Task<Addres> GetByIdAsync(int id);
        Task<Addres> AddAsync(CreateAddressDTO createCategoryDto);
        Task<bool> UpdateAsync(UpdateAddressDTO updateCategoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
