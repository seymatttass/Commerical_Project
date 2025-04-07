using AutoMapper;
using Category.API.Data.Repository;
using Category.API.DTOS.Category;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;

namespace Category.API.services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        // 1️⃣ IHttpClientFactory'yi ekliyoruz
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger,
            IHttpClientFactory httpClientFactory // <--- Yeni eklendi
        )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
            _httpClientFactory = httpClientFactory; // <--- Constructor'da saklanıyor
        }

        public async Task<Data.Entities.Category> AddAsync(CreateCategoryDTO createCategoryDto)
        {
            try
            {
                // 2️⃣ Önce Category API veritabanına kaydet
                var categoryEntity = _mapper.Map<Data.Entities.Category>(createCategoryDto);
                await _categoryRepository.AddAsync(categoryEntity);

                // 3️⃣ Product API'ye REST isteği atmak
                //    - "ProductApi" adında bir HttpClient oluşturuyoruz
                var client = _httpClientFactory.CreateClient("ProductApi");

                //    - Göndereceğimiz JSON gövdesi
                var productApiDto = new
                {
                    Name = categoryEntity.Name,
                    Description = categoryEntity.Description,
                    Active = categoryEntity.Active
                };

                //    - Örnek rota: /api/categories
                var response = await client.PostAsJsonAsync("/api/categories", productApiDto);

                // 4️⃣ Yanıtı kontrol ediyoruz
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Product API'ye kategori eklerken hata oluştu. Status code: {StatusCode}",
                                       response.StatusCode);
                    // Gerekirse burada geri alma veya tekrar deneme (retry) mekanizması düşünebilirsiniz.
                }

                return categoryEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating categories and sending to Product API");
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
