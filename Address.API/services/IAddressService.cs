using Address.API.Data.Entities;
using Address.API.DTOS.Address;

namespace Address.API.services
{
    public interface IAddressService
    {


        Task<IEnumerable<Addres>> GetAllAsync();
        Task<Addres> GetByIdAsync(int id);
        Task<Addres> AddAsync(CreateAddressDTO createAddressDto);
        Task<bool> UpdateAsync(UpdateAddressDTO updateAddressDto);
        Task<bool> DeleteAsync(int id);
    }
}
