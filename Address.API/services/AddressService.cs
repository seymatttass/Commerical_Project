using Address.API.Data.Entities;
using Address.API.Data.Repository;
using AutoMapper;
namespace Address.API.services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;
        public AddressService(
            IAddressRepository addressRepository,
            IMapper mapper,
            ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Addres> AddAsync(CreateAddressDTO createAddressDto)
        {
            try
            {
                var address = _mapper.Map<Addres>(createAddressDto);
                await _addressRepository.AddAsync(address);
                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating address");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _addressRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting address {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Addres>> GetAllAsync()
        {
            try
            {
                return await _addressRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all addresses");
                throw;
            }
        }
        public async Task<Addres> GetByIdAsync(int id)
        {
            try
            {
                return await _addressRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting address {id}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UpdateAddressDTO updateAddressDto)
        {
            try
            {
                // DTO'dan entity'ye dönüştürme
                var address = _mapper.Map<Addres>(updateAddressDto);

                // Adres güncelleme
                return await _addressRepository.UpdateAsync(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating address {updateAddressDto.AddressId}");
                throw;
            }
        }
    }
}