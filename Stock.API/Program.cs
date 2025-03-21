using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Settings;
using Stock.API.Consumers;
using Stock.API.Data;
using Stock.API.Data.Repository;
using Stock.API.DTOS.Validators;
using Stock.API.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DbContext configuration
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));

// Repository and Service registrations
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockService, StockService>();

// Validator'ları kaydet
builder.Services.AddValidatorsFromAssemblyContaining<CreateStockDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateStockDtoValidator>();

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.AddConsumer<StockRollbackMessageConsumer>();
    configurator.AddConsumer<StockCheckedEventConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Order_OrderCreatedQueue, e =>
        e.ConfigureConsumer<OrderCreatedEventConsumer>(context));

        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_RollbackMessageQueue, e =>
        e.ConfigureConsumer<StockRollbackMessageConsumer>(context));

        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_CheckStockQueue, e =>
        e.ConfigureConsumer<StockCheckedEventConsumer>(context));
    });
});


var app = builder.Build();

// Dependency Injection (DI) ile Migration işlemi ve Dummy Data ekleme
using (var scope = app.Services.CreateScope())  // BuildServiceProvider() kullanılmadı
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StockDbContext>();

    // **Migration işlemini uygula** (Tablo yoksa oluşturur, varsa bir şey yapmaz)
    dbContext.Database.Migrate();

    // Eğer stok tablosunda hiç veri yoksa dummy data ekleyelim
    if (!dbContext.Stocks.Any())
    {
        dbContext.Stocks.AddRange(
            new Stock.API.Data.Entities.Stock { ProductId = 1, Count = 200 },
            new Stock.API.Data.Entities.Stock { ProductId = 2, Count = 50 },
            new Stock.API.Data.Entities.Stock { ProductId = 3, Count = 10 },
            new Stock.API.Data.Entities.Stock { ProductId = 4, Count = 10 },
            new Stock.API.Data.Entities.Stock { ProductId = 5, Count = 60 },
            new Stock.API.Data.Entities.Stock { ProductId = 6, Count = 60 },
            new Stock.API.Data.Entities.Stock { ProductId = 7, Count = 60 },
            new Stock.API.Data.Entities.Stock { ProductId = 8, Count = 60 },
            new Stock.API.Data.Entities.Stock { ProductId = 9, Count = 60 },
            new Stock.API.Data.Entities.Stock { ProductId = 10, Count = 60 }
        );

        dbContext.SaveChanges();  // `await` gerektirmez, çünkü `Run()` blok içinde değil
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
