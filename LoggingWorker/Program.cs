using LoggingWorker.Consumers;
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


var host = Host.CreateDefaultBuilder(args)
    .UseSerilog(logger) 
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
