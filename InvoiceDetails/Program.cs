using FluentValidation;
using InvoiceDetails.Data;
using InvoiceDetails.Data.Repository;
using InvoiceDetails.DTOS.Validators;
using InvoiceDetails.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// DbContext configuration
builder.Services.AddDbContext<InvoiceDetailsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));

// Repository and Service registrations
builder.Services.AddScoped<IInvoiceDetailsRepository, InvoiceDetailsRepository>();
builder.Services.AddScoped<IInvoiceDetailsService, InvoiceDetailsService>();

// Validator'larý kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateInvoiceDetailsDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateInvoiceDetailsDtoValidator>();


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
