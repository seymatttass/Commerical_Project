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

builder.Services.AddControllers();

builder.Services.AddDbContext<ShippingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
builder.Services.AddScoped<IShippingService, ShippingService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateShippingDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateShippingDtoValidator>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
