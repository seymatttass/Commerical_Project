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

// HTTP Client Factory
builder.Services.AddHttpClient("Product.API", client =>
{
    client.BaseAddress = new Uri("http://productapi.dev:8080");
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