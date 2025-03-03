using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Data.Entities;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Data.Repository.Orders
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrderDbContext _context;

        public OrdersRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Orderss?> GetByIdAsync(int id)
        {
            return await _context.Orderss.FindAsync(id) ?? null;  // ✅ Null kontrolü eklendi
        }

        public async Task<IEnumerable<Orderss>> GetAllAsync()
        {
            return await _context.Orderss.ToListAsync();
        }

        public async Task<IEnumerable<Orderss>> FindAsync(Expression<Func<Orderss, bool>> predicate)
        {
            return await _context.Orderss.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(Orderss entity)
        {
            await _context.Orderss.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Orderss> entities)
        {
            await _context.Orderss.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await _context.Orderss.FindAsync(id);
            if (entity is null) return false;  // ✅ Null kontrolü eklendi

            _context.Orderss.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveRangeAsync(IEnumerable<Orderss> entities)
        {
            _context.Orderss.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Orderss entity)
        {
            _context.Orderss.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orderss.AnyAsync(o => o.ID == id);
        }
    }
}
