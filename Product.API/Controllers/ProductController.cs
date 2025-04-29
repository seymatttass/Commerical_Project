using Microsoft.AspNetCore.Mvc;
using Product.API.DTOS.ProductDTO.Product;
using Product.API.service.ProductService;
using Microsoft.EntityFrameworkCore;
using Product.API.Data.Entities.ViewModels;
using Product.API.Data;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly ProductDbContext _context;

        public ProductController(
            IProductService productService,
            ILogger<ProductController> logger,
            ProductDbContext context)
        {
            _productService = productService;
            _logger = logger;
            _context = context; 
        }


        [HttpPost("create-all")]
        public async Task<IActionResult> CreateAll([FromBody] CreateAllRequest request)
        {
            if (request.Product == null)
                return BadRequest("Ürün verisi gerekli.");

            if (request.Category == null)
                return BadRequest("Kategori verisi gerekli.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Category.Name.ToLower());

                int categoryId;

                if (existingCategory != null)
                {
                    categoryId = existingCategory.Id;
                }
                else
                {
                    var category = new Product.API.Data.Entities.Category
                    {
                        Name = request.Category.Name,
                        Description = request.Category.Description,
                        Active = request.Category.Active
                    };

                    await _context.Categories.AddAsync(category);
                    await _context.SaveChangesAsync();

                    categoryId = category.Id;
                }

                var product = new Product.API.Data.Entities.Product
                {
                    Name = request.Product.Name,
                    Price = request.Product.Price
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                var productCategory = new Product.API.Data.Entities.ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId
                };

                await _context.ProductCategories.AddAsync(productCategory);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var categoryResult = await _context.Categories.FindAsync(categoryId);

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, new
                {
                    Product = new { Id = product.Id, Name = product.Name, Price = product.Price },
                    Category = new
                    {
                        Id = categoryResult.Id,
                        Name = categoryResult.Name,
                        Description = categoryResult.Description,
                        Active = categoryResult.Active
                    },
                    Relationship = new
                    {
                        Id = productCategory.Id,
                        ProductId = productCategory.ProductId,
                        CategoryId = productCategory.CategoryId
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "İşlem sırasında hata oluştu");
                return Problem($"İşlem sırasında hata oluştu: {ex.Message}", statusCode: 500);
            }
        }








        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm ürünler alınıyor.");
            var product = await _productService.GetAllAsync();
            _logger.LogInformation("Tüm ürünler başarıyla alındı.");
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Ürün alınıyor. ÜrünId: {id}", id);
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Ürün bulunamadı. ÜrünId: {id}", id);
                return NotFound($"{id} ID'li ürün bulunamadı");
            }
            _logger.LogInformation("Ürün başarıyla alındı. ÜrünId: {id}", id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO createProductDto)
        {
            _logger.LogInformation("Yeni ürün oluşturuluyor: {ProductName}", createProductDto.Name);
            var sonuc = await _productService.AddAsync(createProductDto);
            _logger.LogInformation("Ürün başarıyla oluşturuldu. ÜrünId: {id}", sonuc.Id);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO updateProductDto)
        {
            if (id != updateProductDto.ProductId)
            {
                _logger.LogWarning("URL'deki ID ile DTO'daki ID eşleşmiyor. URL ID: {id}, DTO ID: {productId}", id, updateProductDto.ProductId);
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            }

            _logger.LogInformation("Ürün güncelleniyor. ÜrünId: {id}", id);
            var sonuc = await _productService.UpdateAsync(updateProductDto);
            if (!sonuc)
            {
                _logger.LogWarning("Ürün güncellenemedi. ÜrünId: {id}", id);
                return NotFound($"{id} ID'li ürün bulunamadı");
            }
            _logger.LogInformation("Ürün başarıyla güncellendi. ÜrünId: {id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Ürün siliniyor. ÜrünId: {id}", id);
            var sonuc = await _productService.DeleteAsync(id);
            if (!sonuc)
            {
                _logger.LogWarning("Ürün silinemedi. ÜrünId: {id}", id);
                return NotFound($"{id} ID'li ürün bulunamadı");
            }
            _logger.LogInformation("Ürün başarıyla silindi. ÜrünId: {id}", id);
            return NoContent();
        }
    }
}