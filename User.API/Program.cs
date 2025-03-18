using Microsoft.EntityFrameworkCore;
using Users.API.Data;
using Users.API.Data.Repository;
using Users.API.Services;
using Users.API.Mapping;
using FluentValidation;
using Users.API.DTOS.Users;
using Users.API.DTOS.Validators;
using MassTransit;
using Users.API.Consumers;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAutoMapper(typeof(UserAutoMapperProfile));

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserValidators>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<GetUserDetailRequestConsumer>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.GetUserDetailRequestEvent, e =>
        e.ConfigureConsumer<GetUserDetailRequestConsumer>(context));


    });
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
