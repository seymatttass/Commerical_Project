using Microsoft.EntityFrameworkCore;
using Basket.API.Data;
using Basket.API.Data.Repository;
using Basket.API.Services;
using Basket.API.Mapping;
using FluentValidation;
using Basket.API.DTOS;
using Basket.API.DTOS.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// PostgreSQL ile DbContext yap?land?rmas?
builder.Services.AddDbContext<BasketDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection (Ba??ml?l?k Enjeksiyonu)
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();

// AutoMapper Profili Ekleme
builder.Services.AddAutoMapper(typeof(BasketAutoMapperProfile));

// FluentValidation Validatorlar?n? Ekleme
builder.Services.AddValidatorsFromAssemblyContaining<CreateBasketValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBasketValidators>();

// Swagger Deste?i
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Geli?tirme ortam?nda Swagger aktif etme
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
