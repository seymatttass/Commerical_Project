using Stock.API.DTOS.Stock;

namespace Stock.API.services
{
    public interface IStockService 
    {
        Task<IEnumerable<Data.Entities.Stock>> GetAllAsync();
        Task<Data.Entities.Stock> GetByIdAsync(int id);
        Task<Data.Entities.Stock> AddAsync(CreateStockDTO createCategoryDto);
        Task<bool> UpdateAsync(UpdateStockDTO updateCategoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
