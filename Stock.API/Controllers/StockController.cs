using Microsoft.AspNetCore.Mvc;
using Stock.API.DTOS.Stock;
using Stock.API.services;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockService.GetAllAsync();
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _stockService.GetByIdAsync(id);
            if (stock == null)
                return NotFound($"{id} ID'li stok bulunamadı");
            return Ok(stock);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDTO createStockDto)
        {
            var sonuc = await _stockService.AddAsync(createStockDto);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockDTO updateStockDto)
        {
            if (id != updateStockDto.StockId)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor");
            var sonuc = await _stockService.UpdateAsync(updateStockDto);
            if (!sonuc)
                return NotFound($"{id} ID'li stok bulunamadı");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sonuc = await _stockService.DeleteAsync(id);
            if (!sonuc)
                return NotFound($"{id} ID'li stok bulunamadı");
            return NoContent();
        }
    }
}