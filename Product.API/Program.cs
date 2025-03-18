using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
using Shared.Events.BasketEvents;
using Shared.Settings;

using Product.API.service.ProductService;
using MassTransit;
using SagaStateMachine.Service.Consumers;
using Product.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// DbContext configuration
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<CategoryEventConsumer>();
    configurator.AddConsumer<ProductAddedToBasketConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Basket_ProductAddedToBasketQueue, e =>
        e.ConfigureConsumer<ProductAddedToBasketConsumer>(context));

        _configure.ReceiveEndpoint(RabbitMQSettings.Category_CategoryEventQueue, e =>
        e.ConfigureConsumer<CategoryEventConsumer>(context));

    });
});


// Repository and Service registrations
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Validator'ları kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>();


// Repository and Service registrations
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

// Validator'ları kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCategoryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductCategoryDtoValidator>();



// Repository and Service registrations
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Validator'ları kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductDtoValidator>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/create-all", async (ProductDbContext context, CreateAllRequest request) =>
{
    // Verilerin doğruluğunu kontrol et
    if (request.Product == null)
        return Results.BadRequest("Ürün verisi gerekli.");

    if (request.Category == null)
        return Results.BadRequest("Kategori verisi gerekli.");

    // Transaction başlat
    using var transaction = await context.Database.BeginTransactionAsync();

    try
    {
        // 1. Kategoriyi kontrol et, aynı isimli kategori varsa onu kullan
        var existingCategory = await context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Category.Name.ToLower());

        int categoryId;

        if (existingCategory != null)
        {
            // Var olan kategoriyi kullan
            categoryId = existingCategory.Id;
        }
        else
        {
            // Yeni kategori oluştur
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

        // 2. Ürünü oluştur
        var product = new Product.API.Data.Entities.Product
        {
            Name = request.Product.Name,
            Price = request.Product.Price
        };

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        // 3. Ürün-Kategori ilişkisini oluştur
        var productCategory = new Product.API.Data.Entities.ProductCategory
        {
            ProductId = product.Id,
            CategoryId = categoryId
        };

        await context.ProductCategories.AddAsync(productCategory);
        await context.SaveChangesAsync();

        // İşlemleri kaydet
        await transaction.CommitAsync();

        // Sonucu döndür
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
        // Hata durumunda işlemleri geri al
        await transaction.RollbackAsync();
        return Results.Problem($"İşlem sırasında hata oluştu: {ex.Message}", statusCode: 500);
    }
});

app.MapPost("/add-to-basket", async (ProductDbContext context, MassTransit.ISendEndpointProvider sendEndpointProvider) =>
{
    // **1️⃣ - İlk ürünü veritabanından çekelim**
    var firstProduct = await context.Products.OrderBy(p => p.Id).FirstOrDefaultAsync();

    if (firstProduct == null)
    {
        return Results.NotFound("Veritabanında hiç ürün bulunamadı.");
    }

    // **2️⃣ - Event objesini oluşturalım**
    var correlationId = Guid.NewGuid(); // Saga takibi için benzersiz ID

    ProductAddedToBasketRequestEvent productAddedEvent = new(correlationId)
    {
        ProductId = firstProduct.Id,
        Count = 1, // Örnek olarak 1 tane ekliyoruz, isteğe göre değiştirilebilir
        UserId = 1, // Sabit bir kullanıcı ID belirtiyoruz, bunu isteğe göre değiştirebilirsin
        Name = firstProduct.Name,
        Price = firstProduct.Price
    };

    // **3️⃣ - Eventi Saga State Machine kuyruğuna gönderelim**
    var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));
    await sendEndpoint.Send<ProductAddedToBasketRequestEvent>(productAddedEvent);

    return Results.Ok($"Ürün '{firstProduct.Name}' sepete eklendi ve event yayınlandı.");
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
