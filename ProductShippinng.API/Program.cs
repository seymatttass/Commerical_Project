using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductShippinng.API.Data;
using ProductShippinng.API.Data.Repository;
using ProductShippinng.API.DTOS.Validators;
using ProductShippinng.API.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// DbContext configuration
builder.Services.AddDbContext<ProductShippingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));

// Repository and Service registrations
builder.Services.AddScoped<IProductShippingRepository, ProductShippingRepository>();
builder.Services.AddScoped<IProductShippingService, ProductShippingService>();

// Validator'larý kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductShippingDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductShippingDtoValidator>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
