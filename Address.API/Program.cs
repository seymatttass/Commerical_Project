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


builder.Services.AddControllers();

builder.Services.AddDbContext<AddressDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateAddressDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateAddressDtoValidator>();

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
