using FluentValidation;
using InvoiceDetails.Data;
using InvoiceDetails.Data.Repository;
using InvoiceDetails.DTOS.Validators;
using InvoiceDetails.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDbContext<InvoiceDetailsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IInvoiceDetailsRepository, InvoiceDetailsRepository>();
builder.Services.AddScoped<IInvoiceDetailsService, InvoiceDetailsService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateInvoiceDetailsDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateInvoiceDetailsDtoValidator>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
