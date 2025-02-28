using AutoMapper;
using ProductCategory.Data.Repository;
using ProductCategory.DTOS.ProductCategory;

namespace ProductCategory.service
{
    public class ProductCategoryService : IProductCategoryService
    {

            private readonly IProductCategoryRepository _productCategoryRepository;
            private readonly IMapper _mapper;
            private readonly ILogger<ProductCategoryService> _logger;
            public ProductCategoryService(
                IProductCategoryRepository productcategoryRepository,
                IMapper mapper,
                ILogger<ProductCategoryService> logger)
            {
                _productCategoryRepository = productcategoryRepository;
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<Data.Entities.ProductCategory> AddAsync(CreateProductCategoryDTO createCategoryDto)
            {
                try
                {
                    var productcategories = _mapper.Map<Data.Entities.ProductCategory>(createCategoryDto);
                    await _productCategoryRepository.AddAsync(productcategories);
                    return productcategories;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while creating categories");
                    throw;
                }
            }
            public async Task<bool> DeleteAsync(int id)
            {
                try
                {
                    return await _productCategoryRepository.RemoveAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error while deleting categories {id}");
                    throw;
                }
            }
            public async Task<IEnumerable<Data.Entities.ProductCategory>> GetAllAsync()
            {
                try
                {
                    return await _productCategoryRepository.GetAllAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while getting all categories");
                    throw;
                }
            }
            public async Task<Data.Entities.ProductCategory> GetByIdAsync(int id)
            {
                try
                {
                    return await _productCategoryRepository.GetByIdAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error while getting categories {id}");
                    throw;
                }
            }
            public async Task<bool> UpdateAsync(UpdateProductCategoryDTO updateProductCategoryDto)
            {
                try
                {
                    // DTO'dan entity'ye dönüştürme
                    var productCategories = _mapper.Map<Data.Entities.ProductCategory>(updateProductCategoryDto);

                    // Adres güncelleme
                    return await _productCategoryRepository.UpdateAsync(productCategories);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error while updating categories {updateProductCategoryDto.ProductCategoryId}");
                    throw;
                }
            }
        }
    }

