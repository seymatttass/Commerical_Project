using FluentValidation;
using Invoice.API.Consumers;
using Invoice.API.Data;
using Invoice.API.Data.Repository;
using Invoice.API.DTOS.Validators;
using Invoice.API.services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



// DbContext configuration
builder.Services.AddDbContext<InvoiceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));

// Repository and Service registrations
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Validator'larý kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateInvoiceDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateInvoiceDtoValidator>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<CreateInvoiceCommandConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Invoice_CreateInvoiceQueue, e =>
        e.ConfigureConsumer<CreateInvoiceCommandConsumer>(context));


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
