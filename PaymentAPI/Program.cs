using Microsoft.EntityFrameworkCore;
using Payment.API.Data;
using Payment.API.Data.Repository;
using Payment.API.Services;
using Payment.API.Mapping;
using FluentValidation;
using Payment.API.DTOS.Payments;
using Payment.API.DTOS.Validators;
using MassTransit;
using Payment.API.Consumers;
using Shared.Settings;
using Shared.Events.BasketEvents;
using Shared.Events.StockEvents;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;

var builder = WebApplication.CreateBuilder(args);

// Elasticsearch URL'sini alýn
var elasticsearchUrl = builder.Configuration["ElasticConfiguration:Uri"] ?? "http://elasticsearch:9200";

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ServiceName", "Payment.API")
        .WriteTo.Console()
        .WriteTo.File(
            new Serilog.Formatting.Compact.CompactJsonFormatter(),
            "logs/payments-api-.log",
            rollingInterval: RollingInterval.Day)
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"payments-{hostingContext.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
            NumberOfReplicas = 1,
            NumberOfShards = 2
        })
);


builder.Services.AddControllers();

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddAutoMapper(typeof(PaymentAutoMapperProfile));

builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePaymentValidators>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<PaymentStartedEventConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Payment_PaymentStartedQueue, e =>
         e.ConfigureConsumer<PaymentStartedEventConsumer>(context));


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
