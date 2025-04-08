using Category.API.Data;
using Category.API.Data.Repository;
using Category.API.DTOS.Validators;
using Category.API.services;
using Category.API.Services;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;

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

// -- Product API HttpClient yapýlandýrmasý
// appsettings.json içinden ProductApiBaseUrl alýnýyor, yoksa doðrudan container adý kullanýlýyor
// DNS çözümleme için doðru container adý kullanýlmalý
builder.Services.AddHttpClient("Product.API", client =>
{
    // Container adýný doðru kullanýn ve timeout ekleyin
    client.BaseAddress = new Uri("http://Product.API:8080");
    client.Timeout = TimeSpan.FromSeconds(30); // Timeout deðeri
});

var app = builder.Build();

// -- Development ortamý için Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();