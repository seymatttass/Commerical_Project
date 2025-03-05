using Product.API.DTOS.ProductCategoryDTO.ProductCategory;

namespace Product.API.service.ProductCategoryService
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
