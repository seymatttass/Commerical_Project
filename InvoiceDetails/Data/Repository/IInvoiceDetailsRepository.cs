using System.Linq.Expressions;

namespace InvoiceDetails.Data.Repository
{
    public interface IInvoiceDetailsRepository
    {
        Task<Entities.InvoiceDetails> GetByIdAsync(int id);

        Task<IEnumerable<Entities.InvoiceDetails>> GetAllAsync();

        Task<IEnumerable<Entities.InvoiceDetails>> FindAsync(Expression<Func<Entities.InvoiceDetails, bool>> predicate);

        Task AddAsync(Entities.InvoiceDetails entity);
        Task AddRangeAsync(IEnumerable<Entities.InvoiceDetails> entities);

        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.InvoiceDetails> entities);

        Task<bool> UpdateAsync(Entities.InvoiceDetails entity);

        Task<bool> ExistsAsync(int id);
    }
}
