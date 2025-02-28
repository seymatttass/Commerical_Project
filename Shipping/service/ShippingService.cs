using AutoMapper;
using Shipping.API.Data.Repository;
using Shipping.API.DTOS.Shipping;

namespace Shipping.API.service
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository _shippingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ShippingService> _logger;
        public ShippingService(
            IShippingRepository shippingRepository,
            IMapper mapper,
            ILogger<ShippingService> logger)
        {
            _shippingRepository = shippingRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Data.Entities.Shipping> AddAsync(CreateShippingDTO createShippingDto)
        {
            try
            {
                var shipping = _mapper.Map<Data.Entities.Shipping>(createShippingDto);
                await _shippingRepository.AddAsync(shipping);
                return shipping;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating shipping");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _shippingRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting shipping {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Data.Entities.Shipping>> GetAllAsync()
        {
            try
            {
                return await _shippingRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all shipping");
                throw;
            }
        }
        public async Task<Data.Entities.Shipping> GetByIdAsync(int id)
        {
            try
            {
                return await _shippingRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting shipping {id}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UpdateShippingDTO updateShippingDto)
        {
            try
            {
                // DTO'dan entity'ye dönüştürme
                var shipping = _mapper.Map<Data.Entities.Shipping>(updateShippingDto);

                // Adres güncelleme
                return await _shippingRepository.UpdateAsync(shipping);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating shipping {updateShippingDto.ShippingId}");
                throw;
            }

        }
    }
}
