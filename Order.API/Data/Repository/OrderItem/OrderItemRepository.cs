using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Data.Entities;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Data.Repository.OrderItem
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly OrderDbContext _context;

        public OrderItemRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItemss?> GetByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id) ?? null;  // ✅ Null kontrolü eklendi
        }

        public async Task<IEnumerable<OrderItemss>> GetAllAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<IEnumerable<OrderItemss>> FindAsync(Expression<Func<OrderItemss, bool>> predicate)
        {
            return await _context.OrderItems.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(OrderItemss entity)
        {
            await _context.OrderItems.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<OrderItemss> entities)
        {
            await _context.OrderItems.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await _context.OrderItems.FindAsync(id);
            if (entity is null) return false;  // ✅ Null kontrolü eklendi

            _context.OrderItems.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveRangeAsync(IEnumerable<OrderItemss> entities)
        {
            _context.OrderItems.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(OrderItemss entity)
        {
            _context.OrderItems.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.OrderItems.AnyAsync(o => o.ID == id);
        }
    }
}
