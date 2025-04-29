using MassTransit;
using SagaStateMachine.Service;
using SagaStateMachine.Service.StateInstances;
using SagaStateMachine.Service.StateMachines;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine.Service.StateDbContext;
using Shared.Settings;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHostedService<Worker>();


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .EntityFrameworkRepository(options =>
        {
            options.ConcurrencyMode = ConcurrencyMode.Optimistic; 
            options.LockStatementProvider = null; 

            options.AddDbContext<DbContext, OrderStateDbContext>((provider, _builder) =>
            {
                _builder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        });

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.StateMachineQueue, e =>
        {
            e.ConfigureSaga<OrderStateInstance>(context);
        });
    });
}); ;

var host = builder.Build();
host.Run();
