using AutoMapper;
using Category.API.Data.Repository;
using Category.API.DTOS.Category;
using Category.API.services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json;

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
                // Category.API dbye kategoriyi ekliyorum.
                var categoryEntity = _mapper.Map<Data.Entities.Category>(createCategoryDto);
                await _categoryRepository.AddAsync(categoryEntity);

                // Product.API'ye HTTP isteği gönderiyorum.
                await NotifyProductApiAsync(categoryEntity);

                return categoryEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori eklenirken veya Product API'ye bildirilirken hata oluştu.");
                throw;
            }
        }

        private async Task NotifyProductApiAsync(Data.Entities.Category categoryEntity)
        {
            // Product.API URL'ini configden alıyoruz.
            string productApiUrl = _configuration["ProductApiBaseUrl"];

            if (string.IsNullOrEmpty(productApiUrl))
            {
                productApiUrl = "http://Product.API:8080"; // Docker ağındaki container ismi
                _logger.LogWarning("ProductApiBaseUrl ayarı bulunamadı. Varsayılan değer kullanılıyor: {DefaultUrl}", productApiUrl);
            }

            _logger.LogInformation("Product API URL: {ProductApiUrl}", productApiUrl);

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true, 
                AllowAutoRedirect = true,
            };

            // İstek verisi
            var productApiDto = new
            {
                Name = categoryEntity.Name,
                Description = categoryEntity.Description,
                Active = categoryEntity.Active
            };

            string endpoint = "api/categories";
            string fullUrl = $"{productApiUrl.TrimEnd('/')}/{endpoint}";

            _logger.LogInformation("Product API tam URL: {FullUrl}, İstek verileri: {@RequestData}",
                fullUrl, productApiDto);

            try
            {
                using (var client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    // JSON içeriği oluşturuyorum.
                    var jsonContent = JsonSerializer.Serialize(productApiDto);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // HTTP POST isteği gönderiyorum.
                    _logger.LogInformation("HTTP POST isteği gönderiliyor: {Url}", fullUrl);
                    var response = await client.PostAsync(fullUrl, content);

                    _logger.LogInformation("Product API'ye istek gönderildi. Yanıt status kodu: {StatusCode}", response.StatusCode);

                    // Yanıt içeriğini okuyalım.
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Kategori Product API'ye başarıyla bildirildi. Status: {StatusCode}, İçerik: {Content}",
                            response.StatusCode, responseContent);
                    }
                    else
                    {
                        _logger.LogWarning("Product API yanıt verdi ancak başarısız status kodu döndü. Status: {StatusCode}, Yanıt: {Content}",
                            response.StatusCode, responseContent);
                        _logger.LogWarning("Request URL: {RequestUri}", response.RequestMessage?.RequestUri);
                        _logger.LogWarning("Request Method: {Method}", response.RequestMessage?.Method);

                        if (response.StatusCode == HttpStatusCode.Redirect ||
                            response.StatusCode == HttpStatusCode.MovedPermanently ||
                            response.StatusCode == HttpStatusCode.TemporaryRedirect)
                        {
                            var redirectUrl = response.Headers.Location?.ToString();
                            _logger.LogWarning("Yönlendirme algılandı. Yeni URL: {RedirectUrl}", redirectUrl);

                            if (!string.IsNullOrEmpty(redirectUrl))
                            {
                                _logger.LogInformation("Yönlendirmeyi takip ediliyor: {RedirectUrl}", redirectUrl);
                                response = await client.PostAsync(redirectUrl, content);
                                responseContent = await response.Content.ReadAsStringAsync();

                                _logger.LogInformation("Yönlendirme sonrası yanıt: Status: {StatusCode}, İçerik: {Content}",
                                    response.StatusCode, responseContent);
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Product API'ye bağlanırken hata oluştu. URL: {Url}, Hata: {Message}",
                    fullUrl, ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception: {InnerExceptionMessage}", ex.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Product API'ye istek gönderilirken beklenmeyen hata: {ErrorMessage}", ex.Message);
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
                var updated = await _categoryRepository.UpdateAsync(entity);

                if (updated)
                {
                   
                }

                return updated;
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
                var deleted = await _categoryRepository.RemoveAsync(id);

                if (deleted)
                {
                    
                }

                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ID'si {id} olan kategori silinirken hata oluştu.");
                throw;
            }
        }
    }
}