using Category.API.DTOS.Category;

namespace Category.API.services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Data.Entities.Category>> GetAllAsync();
        Task<Data.Entities.Category> GetByIdAsync(int id);
        Task<Data.Entities.Category> AddAsync(CreateCategoryDTO createCategoryDto);
        Task<bool> UpdateAsync(UpdateCategoryDTO updateCategoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
