using System.Linq.Expressions;

namespace Product.API.Data.Repository.ProductRepository
{
    public interface IProductRepository
    {
        Task<Entities.Product> GetByIdAsync(int id);

        Task<IEnumerable<Entities.Product>> GetAllAsync();

        Task<IEnumerable<Entities.Product>> FindAsync(Expression<Func<Entities.Product, bool>> predicate);

        Task AddAsync(Entities.Product entity);
        Task AddRangeAsync(IEnumerable<Entities.Product> entities);

        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Product> entities);

        Task<bool> UpdateAsync(Entities.Product entity);

        Task<bool> ExistsAsync(int id);
    }

}
