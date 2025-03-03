using InvoiceDetails.Data.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace InvoiceDetails.Data.Repository
{
    public class InvoiceDetailsRepository : IInvoiceDetailsRepository
    {

        private readonly InvoiceDetailsDbContext _dbContext;

        public InvoiceDetailsRepository(InvoiceDetailsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Entities.InvoiceDetails entity)
        {
            await _dbContext.InvoiceDetailss.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Entities.InvoiceDetails> entities)
        {
            await _dbContext.InvoiceDetailss.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.InvoiceDetailss.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Entities.InvoiceDetails>> FindAsync(Expression<Func<Entities.InvoiceDetails, bool>> predicate)
        {
            return await _dbContext.InvoiceDetailss.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Entities.InvoiceDetails>> GetAllAsync()
        {
            return await _dbContext.InvoiceDetailss.ToListAsync();
        }

        public async Task<Entities.InvoiceDetails> GetByIdAsync(int id)
        {
            return await _dbContext.InvoiceDetailss.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var invoiceDetails = await GetByIdAsync(id);
            if (invoiceDetails != null)
            {
                return false;
            }

            _dbContext.InvoiceDetailss.Remove(invoiceDetails);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task RemoveRangeAsync(IEnumerable<Entities.InvoiceDetails> entities)
        {
            _dbContext.InvoiceDetailss.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Entities.InvoiceDetails entity)
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
