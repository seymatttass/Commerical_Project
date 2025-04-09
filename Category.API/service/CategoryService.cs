using AutoMapper;
using Category.API.Data.Repository;
using Category.API.DTOS.Category;
using Category.API.services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Data.Entities.Category> AddAsync(CreateCategoryDTO createCategoryDto)
        {
            try
            {
                var categoryEntity = _mapper.Map<Data.Entities.Category>(createCategoryDto);
                await _categoryRepository.AddAsync(categoryEntity);

                // Product.API'ye Docker network üzerinden REST ile bildirim gönderelim.
                var client = _httpClientFactory.CreateClient("Product.API");

                _logger.LogInformation("Product API'ye istek gönderiliyor. Base URL: {BaseUrl}", client.BaseAddress);

                var productApiDto = new
                {
                    Name = categoryEntity.Name,
                    Description = categoryEntity.Description,
                    Active = categoryEntity.Active
                };

                try
                {
                    // API endpoint'e istek gönderelim.
                    var response = await client.PostAsJsonAsync("api/categories", productApiDto);

                    _logger.LogInformation("Product API yanıt verdi. Status: {StatusCode}", response.StatusCode);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogWarning("Product API'ye kategori aktarımı başarısız. StatusCode: {StatusCode}, Error: {Error}",
                            response.StatusCode, errorContent);
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Product API'ye bağlanırken hata oluştu. Adres: {BaseAddress}, Hata: {Message}",
                        client.BaseAddress, ex.Message);
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