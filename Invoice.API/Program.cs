using FluentValidation;
using Invoice.API.Data;
using Invoice.API.Data.Repository;
using Invoice.API.DTOS.Validators;
using Invoice.API.services;
using Microsoft.EntityFrameworkCore;

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
