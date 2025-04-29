using AutoMapper;
using Product.API.Data.Repository.CategoryRepository;
using Product.API.DTOS.CategoryDTO.Category;
using Product.API.Data.Entities;

namespace Product.API.service.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Data.Entities.Category> AddAsync(CreateCategoryDTO createCategoryDto)
        {
            try
            {
                var categories = _mapper.Map<Data.Entities.Category>(createCategoryDto);
                await _categoryRepository.AddAsync(categories);
                return categories;
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
                return await _categoryRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting categories {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Data.Entities.Category>> GetAllAsync()
        {
            try
            {
                return await _categoryRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all categories");
                throw;
            }
        }
        public async Task<Data.Entities.Category> GetByIdAsync(int id)
        {
            try
            {
                return await _categoryRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting categories {id}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UpdateCategoryDTO updateCategoryDto)
        {
            try
            {
                var categories = _mapper.Map<Data.Entities.Category>(updateCategoryDto);

                return await _categoryRepository.UpdateAsync(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating categories {updateCategoryDto.CategoryId}");
                throw;
            }
        }
    }

}
