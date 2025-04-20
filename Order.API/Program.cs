using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Order.API.Data;
using Order.API.Data.Repository.OrderItem;
using Order.API.Data.Repository.Orders;
using Order.API.Services.OrderItemServices;
using Order.API.Services.OrderServices;
using Order.API.Mapping;
using Order.API.DTOS.OrdersDTO.Validators;
using Order.API.DTOS.OrderItemDTO.Validators;
using MassTransit;
using Shared.Events.OrderCreatedEvent;
using Order.API.Consumers;
using Shared.Settings;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;

var builder = WebApplication.CreateBuilder(args);

// Elasticsearch URL'sini alýn
var elasticsearchUrl = builder.Configuration["ElasticConfiguration:Uri"] ?? "http://elasticsearch:9200";


builder.Services.AddControllers();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderItemServices, OrderItemServices>();

builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IOrderServices, OrderServices>();

builder.Services.AddAutoMapper(typeof(OrderAutoMapperProfile));

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrdersValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrdersValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderItemValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderItemValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCompletedEventConsumer>();
    configurator.AddConsumer<OrderFailedEventConsumer>();
    configurator.AddConsumer<CreateOrderCommandConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Order_OrderCompletedQueue, e =>
            e.ConfigureConsumer<OrderCompletedEventConsumer>(context));

        _configure.ReceiveEndpoint(RabbitMQSettings.Order_OrderFailedQueue, e =>
            e.ConfigureConsumer<OrderFailedEventConsumer>(context));

        _configure.ReceiveEndpoint(RabbitMQSettings.Order_OrderCreatedQueue, e =>
            e.ConfigureConsumer<CreateOrderCommandConsumer>(context)); 
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