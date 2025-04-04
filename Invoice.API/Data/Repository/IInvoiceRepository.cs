using System.Linq.Expressions;
using System.Threading.Tasks;
using Invoice.API.Data.Entities;


namespace Invoice.API.Data.Repository
{
    public interface IInvoiceRepository
    {

        Task<Entities.Invoice> GetByIdAsync(int id);

        Task<IEnumerable<Entities.Invoice>> GetAllAsync();

        Task<IEnumerable<Entities.Invoice>> FindAsync(Expression<Func<Entities.Invoice, bool>> predicate);

        Task AddAsync(Entities.Invoice entity);
        Task AddRangeAsync(IEnumerable<Entities.Invoice> entities);

        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Invoice> entities);

        Task<bool> UpdateAsync(Entities.Invoice entity);

        Task<bool> ExistsAsync(int id);

    }
}
