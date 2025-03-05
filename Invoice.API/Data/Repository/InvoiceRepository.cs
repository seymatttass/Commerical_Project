using Invoice.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Invoice.API.Data.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {


        private readonly InvoiceDbContext _dbContext;

        public InvoiceRepository(InvoiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Entities.Invoice entity)
        {
            await _dbContext.Invoices.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Entities.Invoice> entities)
        {
            await _dbContext.Invoices.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Invoices.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Entities.Invoice>> FindAsync(Expression<Func<Entities.Invoice, bool>> predicate)
        {
            return await _dbContext.Invoices.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Entities.Invoice>> GetAllAsync()
        {
            return await _dbContext.Invoices.ToListAsync();
        }

        public async Task<Entities.Invoice> GetByIdAsync(int id)
        {
            return await _dbContext.Invoices.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var invoices = await GetByIdAsync(id);
            if (invoices != null)
            {
                return false;
            }

            _dbContext.Invoices.Remove(invoices);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task RemoveRangeAsync(IEnumerable<Entities.Invoice> entities)
        {
            _dbContext.Invoices.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Entities.Invoice entity)
        {
            var update = await GetByIdAsync(entity.Id);
            if (update == null)
                return false;

            _dbContext.Entry(update).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return true;

        }
    }
}
