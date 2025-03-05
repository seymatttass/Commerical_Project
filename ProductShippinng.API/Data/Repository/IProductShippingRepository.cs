using System.Linq.Expressions;

namespace ProductShippinng.API.Data.Repository
{
    public interface IProductShippingRepository
    {
        Task<Entities.ProductShipping> GetByIdAsync(int id);

        Task<IEnumerable<Entities.ProductShipping>> GetAllAsync();

        Task<IEnumerable<Entities.ProductShipping>> FindAsync(Expression<Func<Entities.ProductShipping, bool>> predicate);

        Task AddAsync(Entities.ProductShipping entity);
        Task AddRangeAsync(IEnumerable<Entities.ProductShipping> entities);

        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.ProductShipping> entities);

        Task<bool> UpdateAsync(Entities.ProductShipping entity);

        Task<bool> ExistsAsync(int id);
    }
}
