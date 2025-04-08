using AutoMapper;
using Category.API.Data.Repository;
using Category.API.DTOS.Category;
using Category.API.services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;

namespace Category.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Data.Entities.Category> AddAsync(CreateCategoryDTO createCategoryDto)
        {
            try
            {
                // 1️⃣ Önce Category veritabanına kaydet
                var categoryEntity = _mapper.Map<Data.Entities.Category>(createCategoryDto);
                await _categoryRepository.AddAsync(categoryEntity);

                // 2️⃣ Product.API’ye REST ile bildirim gönder
                var client = _httpClientFactory.CreateClient("Product.API");

                var productApiDto = new
                {
                    Name = categoryEntity.Name,
                    Description = categoryEntity.Description,
                    Active = categoryEntity.Active
                };

                var response = await client.PostAsJsonAsync("/api/categories", productApiDto);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Product API'ye kategori aktarımı başarısız. StatusCode: {StatusCode}", response.StatusCode);
                    // Gerekirse retry veya rollback mekanizması düşünülebilir
                }

                return categoryEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori eklenirken veya Product API'ye bildirilirken hata oluştu.");
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
                _logger.LogError(ex, "Tüm kategoriler getirilirken hata oluştu.");
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
                _logger.LogError(ex, $"ID'si {id} olan kategori getirilirken hata oluştu.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UpdateCategoryDTO updateCategoryDto)
        {
            try
            {
                var entity = _mapper.Map<Data.Entities.Category>(updateCategoryDto);
                return await _categoryRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ID'si {updateCategoryDto.CategoryId} olan kategori güncellenirken hata oluştu.");
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
                _logger.LogError(ex, $"ID'si {id} olan kategori silinirken hata oluştu.");
                throw;
            }
        }
    }
}