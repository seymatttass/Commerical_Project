using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Swagger deste�ini ekleyin
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Gateway API",
        Version = "v1",
        Description = "API Gateway for Microservices"
    });
});

// Ocelot servislerini ekleyin ve Polly deste�ini sa�lay�n
builder.Services.AddOcelot(builder.Configuration)
    .AddPolly();

var app = builder.Build();

// Geli�tirme ortam�nda Swagger'� aktifle�tirin
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API V1");
    });
}

await app.UseOcelot();

app.Run();