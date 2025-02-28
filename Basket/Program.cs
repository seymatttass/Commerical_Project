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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ? PostgreSQL ile DbContext yapýlandýrmasý
builder.Services.AddDbContext<BasketDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ? Dependency Injection (Baðýmlýlýk Enjeksiyonu)
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();

// ? BasketItem için baðýmlýlýklarý ekle
builder.Services.AddScoped<IBasketItemRepository, BasketItemRepository>();
builder.Services.AddScoped<IBasketItemService, BasketItemService>();

// ? AutoMapper Profili Ekleme
builder.Services.AddAutoMapper(typeof(BasketAutoMapperProfile));

// ? FluentValidation Validatorlarýný Ekleme
builder.Services.AddValidatorsFromAssemblyContaining<CreateBasketValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBasketValidators>();

// ? BasketItem için FluentValidation Validatorlarýný ekle
builder.Services.AddValidatorsFromAssemblyContaining<CreateBasketItemValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBasketItemValidators>();

// ? Swagger Desteði
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ? Geliþtirme ortamýnda Swagger aktif etme
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
