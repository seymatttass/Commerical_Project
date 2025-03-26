using MassTransit;
using SagaStateMachine.Service;
using SagaStateMachine.Service.StateInstances;
using SagaStateMachine.Service.StateMachines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SagaStateMachine.Service.StateDbContext;
using Shared.Settings;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();


builder.Services.AddMassTransit(configurator =>
{
    // Saga State Machine ve Repository konfigürasyonu
    configurator.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .EntityFrameworkRepository(options =>
        {
            // PostgreSQL ile çalýþýrken lock hints'leri devre dýþý býrak
            options.ConcurrencyMode = ConcurrencyMode.Optimistic; // Optimistic concurrency kullan
            options.LockStatementProvider = null; // SQL Server'a özgü kilitler kullanýlmasýn

            options.AddDbContext<DbContext, OrderStateDbContext>((provider, _builder) =>
            {
                _builder.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
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
