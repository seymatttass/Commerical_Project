using Address.API.Consumers;
using Address.API.Data;
using Address.API.Data.Repository;
using Address.API.DTOS.Validators;
using Address.API.services;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// DbContext configuration
builder.Services.AddDbContext<AddressDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));

// Repository and Service registrations
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();

// Validator'larý kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateAddressDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateAddressDtoValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<GetAddressDetailRequestConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.GetAddressDetailRequestEvent, e =>
        e.ConfigureConsumer<GetAddressDetailRequestConsumer>(context));

    });
});



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
