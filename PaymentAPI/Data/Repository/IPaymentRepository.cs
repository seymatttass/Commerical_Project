using System.Linq.Expressions;
using Payment.API.Data.Entities;

namespace Payment.API.Data.Repository
{
    public interface IPaymentRepository
    {
        Task<Paymentt> GetByIdAsync(int id);
        Task<IEnumerable<Paymentt>> GetAllAsync();
        Task<IEnumerable<Paymentt>> FindAsync(Expression<Func<Paymentt, bool>> predicate);
        Task AddAsync(Paymentt entity);
        Task AddRangeAsync(IEnumerable<Paymentt> entities);
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Paymentt> entities);
        Task<bool> UpdateAsync(Paymentt entity);
        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<Paymentt>> GetPaymentsByOrderIdAsync(int orderId);

        Task<bool> HasPaymentForBasketAsync(int basketId);
    }
}
