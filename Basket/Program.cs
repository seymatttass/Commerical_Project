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

        // Basket_ProductAddedToBasketQueue kuyruðuna gönderim yapýlandýrmasý
        _configure.Send<ProductAddedToBasketRequestEvent>(x =>
        {
            // StateMachineQueue'ya göndermeli, çünkü event'i saga'nýn almasý gerekiyor
            x.UseRoutingKeyFormatter(context => RabbitMQSettings.StateMachineQueue);
        });
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
    IBasketItemRepository basketItemRepository, ISendEndpointProvider sendEndpointProvider) =>
{
    // Sepeti kontrol et veya oluþtur
    var basket = await basketRepository.GetByUserIdAsync(model.UserId);
    if (basket == null)
    {
        // Yeni sepet oluþtur
        basket = new Baskett
        {
            UserId = model.UserId,
        };
        await basketRepository.AddAsync(basket);
    }

    // Sepete ürün ekle
    var basketItem = new BasketItem
    {
        ProductId = model.ProductId,
        Count = model.Count,
        Price = model.Price,
    };
    await basketItemRepository.AddAsync(basketItem);

    // ProductAddedToBasketRequestEvent oluþtur ve gönder
    var correlationId = Guid.NewGuid();
    var productAddedEvent = new ProductAddedToBasketRequestEvent(correlationId)
    {
        ProductId = model.ProductId,
        Count = model.Count,
        UserId = model.UserId,
        Price = model.Price
    };

    // Event'i Saga State Machine'e gönder
    var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(
        new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));
    await sendEndpoint.Send<ProductAddedToBasketRequestEvent>(productAddedEvent);

    return Results.Ok(new
    {
        BasketId = basket.ID,
        BasketItemId = basketItem.ID,
        Message = "Ürün sepete eklendi ve event gönderildi",
        CorrelationId = correlationId
    });
});





app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();