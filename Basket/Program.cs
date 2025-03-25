using Microsoft.EntityFrameworkCore;
using Basket.API.Data;
using Basket.API.Mapping;
using FluentValidation;
using Basket.API.DTOS.BasketDTO.Validators;
using Basket.API.Data.Repository.Basket;
using Basket.API.Services.BasketService;
using Basket.API.Data.Repository;
using Basket.API.DTOS.Validators;
using Basket.API.Services.BasketServices;
using Basket.API.Services;
using MassTransit;
using Shared.Events.BasketEvents;
using Shared.Settings;
using Basket.API.Data.ViewModels;
using Basket.API.Data.Entities;
using System.Linq;
using Basket.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BasketDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "BasketCache:";
});


builder.Services.AddMassTransit(configurator =>
{

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

    });
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddScoped<IBasketItemRepository, BasketItemRepository>();
builder.Services.AddScoped<IBasketItemService, BasketItemService>();

builder.Services.AddAutoMapper(typeof(BasketAutoMapperProfile));

builder.Services.AddValidatorsFromAssemblyContaining<CreateBasketValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBasketValidators>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBasketItemValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBasketItemValidators>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapPost("/add-to-basket", async (AddToBasketVM model, IBasketRepository basketRepository,
    IBasketItemRepository basketItemRepository, BasketDbContext context, ISendEndpointProvider sendEndpointProvider) =>
{
    // Sepet oluþtur
    Baskett baskett = new()
    {
        UserId = model.UserId,
        TotalPrice = model.BasketItems.Sum(bi => bi.Price * bi.Count),
        BasketItems = model.BasketItems.Select(bi => new BasketItem
        {
            Price = bi.Price,
            Count = bi.Count,
            ProductId = bi.ProductId,
        }).ToList(),
    };

    await context.Baskets.AddAsync(baskett);
    await context.SaveChangesAsync();



    // ProductAddedToBasketRequestEvent oluþtur 
    ProductAddedToBasketRequestEvent productAddedEvent = new()
    {
        ProductId = model.ProductId,
        Count = model.Count,
        UserId = baskett.UserId,
        Price = model.Price,
        BasketItemMessages = baskett.BasketItems.Select(bi => new Shared.Messages.BasketItemMessage
        {
            Price = bi.Price,
            Count = bi.Count,
            ProductId = bi.ProductId,
        }).ToList(),
    };

    // Event'i Saga State Machine'e gönder
    var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(
        new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));
    await sendEndpoint.Send<ProductAddedToBasketRequestEvent>(productAddedEvent);
});


// Diðer middleware tanýmlamalarý...
app.UseHttpsRedirection();
app.UseRouting(); // Eðer yoksa ekleyin
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// veya daha basit þekilde:
app.MapControllers();

app.Run();