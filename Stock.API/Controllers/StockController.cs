using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stock.API.DTOS.Stock;
using Stock.API.services;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<StockController> _logger;

        public StockController(IStockService stockService, ILogger<StockController> logger)
        {
            _stockService = stockService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all stock items.");
            var stocks = await _stockService.GetAllAsync();
            if (stocks == null || !stocks.Any())
            {
                _logger.LogWarning("No stock items found.");
            }
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Fetching stock item with ID: {id}");
            var stock = await _stockService.GetByIdAsync(id);
            if (stock == null)
            {
                _logger.LogWarning($"Stock item with ID: {id} not found.");
                return NotFound($"{id} ID'li stok bulunamadı");
            }
            return Ok(stock);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDTO createStockDto)
        {
            _logger.LogInformation("Creating new stock item.");
            var sonuc = await _stockService.AddAsync(createStockDto);
            _logger.LogInformation($"Stock item created with ID: {sonuc.Id}");
            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockDTO updateStockDto)
        {
            _logger.LogInformation($"Updating stock item with ID: {id}");
            if (id != updateStockDto.StockId)
            {
                _logger.LogWarning($"ID mismatch: URL ID = {id}, DTO ID = {updateStockDto.StockId}");
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            }
            var sonuc = await _stockService.UpdateAsync(updateStockDto);
            if (!sonuc)
            {
                _logger.LogWarning($"Stock item with ID: {id} not found for update.");
                return NotFound($"{id} ID'li stok bulunamadı");
            }
            _logger.LogInformation($"Stock item with ID: {id} updated successfully.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting stock item with ID: {id}");
            var sonuc = await _stockService.DeleteAsync(id);
            if (!sonuc)
            {
                _logger.LogWarning($"Stock item with ID: {id} not found for deletion.");
                return NotFound($"{id} ID'li stok bulunamadı");
            }
            _logger.LogInformation($"Stock item with ID: {id} deleted successfully.");
            return NoContent();
        }
    }
}
