using AutoMapper;
using ProductShipping.Data.Repository;
using ProductShipping.DTOS.ProductShipping;

namespace ProductShipping.service
{
    public class ProductShippingService : IProductShippingService
    {
        private readonly IProductShippingRepository _productshippingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductShippingService> _logger;
        public ProductShippingService(
            IProductShippingRepository productshippingRepository,
            IMapper mapper,
            ILogger<ProductShippingService> logger)
        {
            _productshippingRepository = productshippingRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Data.Entities.ProductShipping> AddAsync(CreateProductShippingDTO createproductShippingDto)
        {
            try
            {
                var shipping = _mapper.Map<Data.Entities.ProductShipping>(createproductShippingDto);
                await _productshippingRepository.AddAsync(shipping);
                return shipping;
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
                return await _productshippingRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting ProductShipping {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Data.Entities.ProductShipping>> GetAllAsync()
        {
            try
            {
                return await _productshippingRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all ProductShipping");
                throw;
            }
        }
        public async Task<Data.Entities.ProductShipping> GetByIdAsync(int id)
        {
            try
            {
                return await _productshippingRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting ProductShipping {id}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UpdateProductShippingDTO updateProductShippingDto)
        {
            try
            {
                // DTO'dan entity'ye dönüştürme
                var productshipping = _mapper.Map<Data.Entities.ProductShipping>(updateProductShippingDto);

                // Adres güncelleme
                return await _productshippingRepository.UpdateAsync(productshipping);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating ProductShipping {updateProductShippingDto.ShippingId}");
                throw;
            }

        }
    }
}
