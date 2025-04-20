using LoggingWorker.Consumers;
using Logging.Shared.Models;
using MassTransit;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using LoggingWorker;

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"central-logs-{DateTime.UtcNow:yyyy-MM}"
    })
    .CreateLogger();

// ? Eski modeldeki gibi builder yerine doðrudan Host ile baþlanýr
var host = Host.CreateDefaultBuilder(args)
    .UseSerilog(logger) // Artýk doðru yerde
    .ConfigureServices((context, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<LoggingEventConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(new Uri(context.Configuration["RabbitMQ"]), _ => { });

                cfg.ReceiveEndpoint("log-queue", e =>
                {
                    e.ConfigureConsumer<LoggingEventConsumer>(ctx);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
