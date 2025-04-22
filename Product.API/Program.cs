using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product.API.Data;
using Product.API.Data.Entities.ViewModels;
using Product.API.Data.Repository.CategoryProductRepository;
using Product.API.Data.Repository.CategoryRepository;
using Product.API.Data.Repository.ProductRepository;
using Product.API.DTOS.CategoryDTO.Validator;
using Product.API.DTOS.ProductCategoryDTO.Validator;
using Product.API.DTOS.ProductDTO.Validator;
using Product.API.service.CategoryService;
using Product.API.service.ProductCategoryService;
using Product.API.service.ProductService;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Elasticsearch URL'sini alın
var elasticsearchUrl = builder.Configuration["ElasticConfiguration:Uri"] ?? "http://elasticsearch:9200";

builder.Services.AddControllers();

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repository ve Service kayıtları
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>();

builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCategoryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductCategoryDtoValidator>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductDtoValidator>();





// JWT Authentication ekleyin
string authenticationProviderKey = "TestKey";
SymmetricSecurityKey signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Security"]));

builder.Services.AddAuthentication(options =>
    options.DefaultAuthenticateScheme = authenticationProviderKey)
    .AddJwtBearer(authenticationProviderKey, options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signInKey,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true
        };
    });




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/create-all", async (ProductDbContext context, CreateAllRequest request) =>
{
    if (request.Product == null)
        return Results.BadRequest("Ürün verisi gerekli.");

    if (request.Category == null)
        return Results.BadRequest("Kategori verisi gerekli.");

    // Transaction başlatılsın.
    using var transaction = await context.Database.BeginTransactionAsync();

    try
    {
        // 1. Kategoriyi kontrol et, aynı isimli kategori varsa onu kullanıyorum.
        var existingCategory = await context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Category.Name.ToLower());

        int categoryId;

        if (existingCategory != null)
        {
            // Var olan kategoriyi kullanıyorum.
            categoryId = existingCategory.Id;
        }
        else
        {
            // Yeni kategori oluşturalım.
            var category = new Product.API.Data.Entities.Category
            {
                Name = request.Category.Name,
                Description = request.Category.Description,
                Active = request.Category.Active
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            categoryId = category.Id;
        }

        var product = new Product.API.Data.Entities.Product
        {
            Name = request.Product.Name,
            Price = request.Product.Price
        };

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        var productCategory = new Product.API.Data.Entities.ProductCategory
        {
            ProductId = product.Id,
            CategoryId = categoryId
        };

        await context.ProductCategories.AddAsync(productCategory);
        await context.SaveChangesAsync();

        await transaction.CommitAsync();

        var categoryResult = await context.Categories.FindAsync(categoryId);

        return Results.Created("/create-all", new
        {
            Product = new { Id = product.Id, Name = product.Name, Price = product.Price },
            Category = new
            {
                Id = categoryResult.Id,
                Name = categoryResult.Name,
                Description = categoryResult.Description,
                Active = categoryResult.Active
            },
            Relationship = new
            {
                Id = productCategory.Id,
                ProductId = productCategory.ProductId,
                CategoryId = productCategory.CategoryId
            }
        });
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        return Results.Problem($"İşlem sırasında hata oluştu: {ex.Message}", statusCode: 500);
    }
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();