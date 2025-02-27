using AutoMapper;
using Stock.API.Data.Repository;
using Stock.API.DTOS.Stock;

namespace Stock.API.services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StockService> _logger;
        public StockService(
            IStockRepository stockRepository,
            IMapper mapper,
            ILogger<StockService> logger)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Data.Entities.Stock> AddAsync(CreateStockDTO createStockDto)
        {
            try
            {
                var address = _mapper.Map<Data.Entities.Stock>(createStockDto);
                await _stockRepository.AddAsync(address);
                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating address");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _stockRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting address {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Data.Entities.Stock>> GetAllAsync()
        {
            try
            {
                return await _stockRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all addresses");
                throw;
            }
        }
        public async Task<Data.Entities.Stock> GetByIdAsync(int id)
        {
            try
            {
                return await _stockRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting address {id}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UpdateStockDTO updateStockDto)
        {
            try
            {
                // DTO'dan entity'ye dönüştürme
                var address = _mapper.Map<Data.Entities.Stock>(updateStockDto);

                // Adres güncelleme
                return await _stockRepository.UpdateAsync(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating address {updateStockDto.StockId}");
                throw;
            }
        }
    }
}
