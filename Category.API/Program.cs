using Category.API.Data;
using Category.API.Data.Repository;
using Category.API.DTOS.Validators;
using Category.API.services;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// -- Veritabaný baðlantýsý
builder.Services.AddDbContext<CategoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// -- AutoMapper, Repository, Service
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// -- FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>();

// -- Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -- MassTransit (RabbitMQ)
builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);
    });
});

// 1?? Product API'ye REST istekleri atmak için HttpClient kaydý
// appsettings.json veya environment variable içinden "ProductApiBaseUrl" çekiyoruz.
// Eðer deðer boþ gelirse, localhost:6001 gibi bir varsayýlan deðer atayabiliriz.
var productApiBaseUrl = builder.Configuration["ProductApiBaseUrl"] 
                       ?? "https://localhost:7234"; // Fallback örneði

builder.Services.AddHttpClient("ProductApi", client =>
{
    client.BaseAddress = new Uri(productApiBaseUrl);
});

var app = builder.Build();

// -- Dev ortamý için Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
