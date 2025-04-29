using AutoMapper;
using Category.API.Data.Repository;
using Category.API.DTOS.Category;
using Category.API.services;
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
                // 1. Önce Category.API veritabanına kategoriyi ekle
                _logger.LogInformation("Kategori veritabanına ekleniyor: {CategoryName}", createCategoryDto.Name);
                var categoryEntity = _mapper.Map<Data.Entities.Category>(createCategoryDto);
                await _categoryRepository.AddAsync(categoryEntity);
                _logger.LogInformation("Kategori veritabanına başarıyla eklendi. ID: {CategoryId}", categoryEntity.Id);

                // 2. Gateway API üzerinden Product.API'ye bildirim gönder
                _logger.LogInformation("Product API'ye bildirim gönderiliyor: {CategoryName}", categoryEntity.Name);
                await NotifyProductApiViaGatewayAsync(categoryEntity);

                return categoryEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori eklenirken veya Product API'ye bildirilirken hata oluştu. Kategori: {CategoryName}", createCategoryDto.Name);
                throw;
            }
        }

        private async Task NotifyProductApiViaGatewayAsync(Data.Entities.Category categoryEntity)
        {
            // Docker Compose'da belirtilen gerçek container isimlerini kullan
            string gatewayUrl = "http://Gateway.API:8080";

            // Endpoint path
            string endpoint = "/gateway/product/categories";
            string fullUrl = $"{gatewayUrl.TrimEnd('/')}{endpoint}";

            _logger.LogInformation("Gateway URL: {GatewayUrl}", gatewayUrl);
            _logger.LogInformation("Gateway tam URL: {FullUrl}", fullUrl);

            // İstek verisini hazırla
            var categoryDto = new
            {
                Name = categoryEntity.Name,
                Description = categoryEntity.Description,
                Active = categoryEntity.Active
            };

            _logger.LogInformation("Gönderilecek kategori verisi: {@CategoryData}", categoryDto);

            // HTTP Client oluştur
            try
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    // Timeout ayarla (30 saniye)
                    client.Timeout = TimeSpan.FromSeconds(30);

                    // İstek headerlarını ayarla
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    // JSON içeriği oluştur
                    var jsonContent = JsonSerializer.Serialize(categoryDto);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Gönderilecek JSON içeriğini logla
                    _logger.LogInformation("Gönderilecek JSON içeriği: {JsonContent}", jsonContent);

                    // HTTP POST isteği gönder
                    _logger.LogInformation("HTTP POST isteği gönderiliyor: {Url}", fullUrl);
                    var response = await client.PostAsync(fullUrl, content);

                    // Yanıt durumunu logla
                    _logger.LogInformation("Gateway'e istek gönderildi. Yanıt status kodu: {StatusCode}", response.StatusCode);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Gateway başarısız yanıt döndü. Status: {StatusCode}", response.StatusCode);

                        // Gateway hata verirse, direkt Product API'ye göndermeyi deneyelim
                        string productApiUrl = "http://Product.API:8080";
                        string productEndpoint = "/api/categories";
                        string productFullUrl = $"{productApiUrl.TrimEnd('/')}{productEndpoint}";

                        _logger.LogInformation("Gateway başarısız oldu. Direkt Product API'ye istek deneniyor: {Url}", productFullUrl);

                        var productResponse = await client.PostAsync(productFullUrl, content);
                        _logger.LogInformation("Product API'ye direkt istek gönderildi. Yanıt: {StatusCode}", productResponse.StatusCode);
                    }

                    // Yanıt içeriğini oku
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Gateway yanıt içeriği: {Content}", responseContent);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Gateway'e istek gönderilirken HTTP hatası oluştu: {ErrorMessage}", ex.Message);

                // HTTP isteği sırasında hata olursa, direkt Product API'ye göndermeyi deneyelim
                try
                {
                    string productApiUrl = "http://Product.API:8080";
                    string productEndpoint = "/api/categories";
                    string productFullUrl = $"{productApiUrl.TrimEnd('/')}{productEndpoint}";

                    _logger.LogInformation("HTTP hatası oluştu. Direkt Product API'ye istek deneniyor: {Url}", productFullUrl);

                    using (var backupClient = _httpClientFactory.CreateClient())
                    {
                        var jsonContent = JsonSerializer.Serialize(categoryDto);
                        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        var productResponse = await backupClient.PostAsync(productFullUrl, content);
                        _logger.LogInformation("Product API'ye direkt istek gönderildi. Yanıt: {StatusCode}", productResponse.StatusCode);
                    }
                }
                catch (Exception backupEx)
                {
                    _logger.LogError(backupEx, "Product API'ye direkt istek gönderilirken hata oluştu: {ErrorMessage}", backupEx.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gateway'e istek gönderilirken genel hata oluştu: {ErrorMessage}", ex.Message);
            }
        }

        // Diğer metodlar...
        public async Task<IEnumerable<Data.Entities.Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Data.Entities.Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(UpdateCategoryDTO updateCategoryDto)
        {
            try
            {
                var entity = _mapper.Map<Data.Entities.Category>(updateCategoryDto);
                var updated = await _categoryRepository.UpdateAsync(entity);

                if (updated)
                {
                    // Kategori güncellendiğinde Product API'ye de bildir
                    await NotifyProductApiViaGatewayAsync(entity);
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
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                    return false;

                var deleted = await _categoryRepository.RemoveAsync(id);

                if (deleted)
                {
                    // Silme işlemi için Product API'ye bildirim gönderme eklenebilir
                    // Bu kısım geliştirilebilir
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