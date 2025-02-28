using ProductCategory.DTOS.ProductCategory;

namespace ProductCategory.service
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<Data.Entities.ProductCategory>> GetAllAsync();
        Task<Data.Entities.ProductCategory> GetByIdAsync(int id);
        Task<Data.Entities.ProductCategory> AddAsync(CreateProductCategoryDTO createProductCategoryDto);
        Task<bool> UpdateAsync(UpdateProductCategoryDTO updateProductCategoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
