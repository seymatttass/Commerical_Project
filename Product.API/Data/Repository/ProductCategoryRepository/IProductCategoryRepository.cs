using System.Linq.Expressions;

namespace Product.API.Data.Repository.CategoryProductRepository
{
    public interface IProductCategoryRepository
    {
        Task<Entities.ProductCategory> GetByIdAsync(int id);

        Task<IEnumerable<Entities.ProductCategory>> GetAllAsync();

        Task<IEnumerable<Entities.ProductCategory>> FindAsync(Expression<Func<Entities.ProductCategory, bool>> predicate);

        Task AddAsync(Entities.ProductCategory entity);
        Task AddRangeAsync(IEnumerable<Entities.ProductCategory> entities);

        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.ProductCategory> entities);

        Task<bool> UpdateAsync(Entities.ProductCategory entity);

        Task<bool> ExistsAsync(int id);
    }
}
