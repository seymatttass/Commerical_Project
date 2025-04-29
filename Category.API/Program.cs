using Category.API.Data;
using Category.API.Data.Repository;
using Category.API.DTOS.Validators;
using Category.API.services;
using Category.API.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var elasticsearchUrl = builder.Configuration["ElasticConfiguration:Uri"] ?? "http://elasticsearch:9200";


builder.Services.AddControllers();

builder.Services.AddDbContext<CategoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Product.API'ye eriþim 
builder.Services.AddHttpClient("Product.API", client =>
{
    var baseUrl = builder.Configuration["ProductApiBaseUrl"] ?? "http://Product.API:8080";
    client.BaseAddress = new Uri(baseUrl);
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