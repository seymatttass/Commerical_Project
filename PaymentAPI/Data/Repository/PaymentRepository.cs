using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Payment.API.Data.Entities;

namespace Payment.API.Data.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _dbContext;

        public PaymentRepository(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Paymentt entity)
        {
            await _dbContext.Payments.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Paymentt> entities)
        {
            await _dbContext.Payments.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Payments.AnyAsync(x => x.ID == id);
        }

        public async Task<IEnumerable<Paymentt>> FindAsync(Expression<Func<Paymentt, bool>> predicate)
        {
            return await _dbContext.Payments.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Paymentt>> GetAllAsync()
        {
            return await _dbContext.Payments.ToListAsync();
        }

        public async Task<Paymentt> GetByIdAsync(int id)
        {
            return await _dbContext.Payments.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment == null)
            {
                return false;
            }

            _dbContext.Payments.Remove(payment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task RemoveRangeAsync(IEnumerable<Paymentt> entities)
        {
            _dbContext.Payments.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Paymentt entity)
        {
            var update = await GetByIdAsync(entity.ID);
            if (update == null)
                return false;

            _dbContext.Entry(update).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Paymentt>> GetPaymentsByOrderIdAsync(int orderId)
        {
            return await _dbContext.Payments.Where(p => p.OrderId == orderId).ToListAsync();
        }

        public async Task<bool> HasPaymentForBasketAsync(int basketId)
        {
            return await _dbContext.Payments.AnyAsync(p => p.BasketId == basketId);
        }
    }
}
