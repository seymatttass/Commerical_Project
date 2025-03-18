using MassTransit;
using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.Data.Entities;
using Shared.Events.CategoryEvents;
using System;
using System.Threading.Tasks;

namespace Product.API.Consumers
{
    public class CategoryEventConsumer :
        IConsumer<CategoryCreatedEvent>,
        IConsumer<CategoryUpdatedEvent>,
        IConsumer<CategoryDeletedEvent>
    {
        private readonly ProductDbContext _context;

        public CategoryEventConsumer(ProductDbContext context)
        {
            _context = context;
        }

        // ✅ Kategori Oluşturma Event'ini Dinler
        public async Task Consume(ConsumeContext<CategoryCreatedEvent> context)
        {
            var categoryCreatedEvent = context.Message;

            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryCreatedEvent.CategoryId);

            if (existingCategory == null)
            {
                var newCategory = new Category
                {
                    Id = categoryCreatedEvent.CategoryId,
                    Name = categoryCreatedEvent.Name,
                    Description = categoryCreatedEvent.Description,
                    Active = categoryCreatedEvent.Active
                };

                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Category created in Product API: {categoryCreatedEvent.CategoryId}");
            }
        }

        // ✅ Kategori Güncelleme Event'ini Dinler
        public async Task Consume(ConsumeContext<CategoryUpdatedEvent> context)
        {
            var categoryUpdatedEvent = context.Message;

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryUpdatedEvent.CategoryId);

            if (category != null)
            {
                category.Name = categoryUpdatedEvent.Name;
                category.Description = categoryUpdatedEvent.Description;
                category.Active = categoryUpdatedEvent.Active;

                await _context.SaveChangesAsync();

                Console.WriteLine($"Category updated in Product API: {categoryUpdatedEvent.CategoryId}");
            }
        }

        // ✅ Kategori Silme Event'ini Dinler
        public async Task Consume(ConsumeContext<CategoryDeletedEvent> context)
        {
            var categoryDeletedEvent = context.Message;

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryDeletedEvent.CategoryId);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Category deleted in Product API: {categoryDeletedEvent.CategoryId}");
            }
        }
    }
}
