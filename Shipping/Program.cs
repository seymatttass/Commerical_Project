using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Shared.Settings;
using Shipping.API.Consumers;
using Shipping.API.Data;
using Shipping.API.Data.Repository;
using Shipping.API.DTOS.Validators;
using Shipping.API.service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// DbContext configuration
builder.Services.AddDbContext<ShippingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));

// Repository and Service registrations
builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
builder.Services.AddScoped<IShippingService, ShippingService>();

// Validator'larý kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateShippingDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateShippingDtoValidator>();


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


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<ShippingCompletedEventConsumer>();
    configurator.AddConsumer<ShippingCreatedEventConsumer>();
    configurator.AddConsumer<ShippingFailedEventConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Invoice_CreateInvoiceQueue, e =>
        e.ConfigureConsumer<ShippingCompletedEventConsumer>(context));

        _configure.ReceiveEndpoint(RabbitMQSettings.Invoice_CreateInvoiceQueue, e =>
        e.ConfigureConsumer<ShippingCreatedEventConsumer>(context));

        _configure.ReceiveEndpoint(RabbitMQSettings.Invoice_CreateInvoiceQueue, e =>
        e.ConfigureConsumer<ShippingFailedEventConsumer>(context));

    });


});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
