using MassTransit;
using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.Data.Entities;
using Shared.Events.CategoryEvents;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CategoryEventConsumer> _logger;

        public CategoryEventConsumer(ProductDbContext context, ILogger<CategoryEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

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

                _logger.LogInformation("Category created in Product API: {CategoryId}", categoryCreatedEvent.CategoryId);
            }
        }

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

                _logger.LogInformation("Category updated in Product API: {CategoryId}", categoryUpdatedEvent.CategoryId);
            }
        }

        public async Task Consume(ConsumeContext<CategoryDeletedEvent> context)
        {
            var categoryDeletedEvent = context.Message;
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryDeletedEvent.CategoryId);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Category deleted in Product API: {CategoryId}", categoryDeletedEvent.CategoryId);
            }
        }
    }
}
