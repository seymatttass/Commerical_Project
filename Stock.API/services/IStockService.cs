using Stock.API.DTOS.Stock;

namespace Stock.API.services
{
    public interface IStockService 
    {
        Task<IEnumerable<Data.Entities.Stock>> GetAllAsync();
        Task<Data.Entities.Stock> GetByIdAsync(int id);
        Task<Data.Entities.Stock> AddAsync(CreateStockDTO createStockDto);
        Task<bool> UpdateAsync(UpdateStockDTO updateStockDto);
        Task<bool> DeleteAsync(int id);
    }
}
