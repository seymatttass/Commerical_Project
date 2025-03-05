using AutoMapper;
using Product.API.Data.Repository.ProductRepository;
using Product.API.DTOS.ProductDTO.Product;

namespace Product.API.service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Data.Entities.Product> AddAsync(CreateProductDTO createProductDto)
        {
            try
            {
                var products = _mapper.Map<Data.Entities.Product>(createProductDto);
                await _productRepository.AddAsync(products);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating ürün");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _productRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting ürün {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Data.Entities.Product>> GetAllAsync()
        {
            try
            {
                return await _productRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all ürün");
                throw;
            }
        }
        public async Task<Data.Entities.Product> GetByIdAsync(int id)
        {
            try
            {
                return await _productRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting ürün {id}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDto)
        {
            try
            {
                // DTO'dan entity'ye dönüştürme
                var products = _mapper.Map<Data.Entities.Product>(updateProductDto);

                // Adres güncelleme
                return await _productRepository.UpdateAsync(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating ürün {updateProductDto.ProductId}");
                throw;
            }
        }

    }

}
