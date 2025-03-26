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

var builder = WebApplication.CreateBuilder(args);

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
