using AutoMapper;
using ProductShippinng.API.Data.Entities;
using ProductShippinng.API.Data.Repository;
using ProductShippinng.API.DTOS.ProductShipping;

namespace ProductShippinng.API.services
{
    public class ProductShippingService  : IProductShippingService
    {

        private readonly IProductShippingRepository _productShippingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductShippingService> _logger;
        public ProductShippingService(
            IProductShippingRepository productShippingRepository,
            IMapper mapper,
            ILogger<ProductShippingService> logger)
        {
            _productShippingRepository = productShippingRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ProductShipping> AddAsync(CreateProductShippingDTO createProductShippingDto)
        {
            try
            {
                var address = _mapper.Map<ProductShipping>(createProductShippingDto);
                await _productShippingRepository.AddAsync(address);
                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating ProductShipping");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _productShippingRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting address {id}");
                throw;
            }
        }
        public async Task<IEnumerable<ProductShipping>> GetAllAsync()
        {
            try
            {
                return await _productShippingRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all addresses");
                throw;
            }
        }
        public async Task<ProductShipping> GetByIdAsync(int id)
        {
            try
            {
                return await _productShippingRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting address {id}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UpdateProductShippingDTO updateAddressDto)
        {
            try
            {
                // DTO'dan entity'ye dönüştürme
                var address = _mapper.Map<ProductShipping>(updateAddressDto);

                // Adres güncelleme
                return await _productShippingRepository.UpdateAsync(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating address {updateAddressDto.ProductShippingId}");
                throw;
            }
        }
    }
}
