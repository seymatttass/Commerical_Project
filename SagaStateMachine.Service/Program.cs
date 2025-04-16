using MassTransit;
using SagaStateMachine.Service;
using SagaStateMachine.Service.StateInstances;
using SagaStateMachine.Service.StateMachines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SagaStateMachine.Service.StateDbContext;
using Shared.Settings;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Builder;



var builder = WebApplication.CreateBuilder(args);


// Serilog yapýlandýrmasý
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ServiceName", "SagaStateMachine.Service")
        .WriteTo.Console()
        .WriteTo.File(
            new Serilog.Formatting.Compact.CompactJsonFormatter(),
            "logs/saga-state-machine-api-.log",
            rollingInterval: RollingInterval.Day)
);







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
