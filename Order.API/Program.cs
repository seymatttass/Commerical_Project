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

var builder = WebApplication.CreateBuilder(args);

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