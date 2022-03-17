using EventTicketingSystemZaven_CommandProcessor;
using EventTicketingSystemZaven_CommandProcessor.Models;
using EventTicketingSystemZaven_CommandProcessor.Services;
using EventTicketingSystemZaven_CommandProcessor.Services.Interfaces;
using EventTicketingSystemZaven_CommandProcessor.Services.OperationProcessors;
using EventTicketingSystemZaven_Shared.Models;
using EventTicketingSystemZaven_Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();

        services.AddHostedService<EventHubListener>();

        services.AddSingleton(context.Configuration.GetSection(nameof(EventHubConfiguration)).Get<EventHubConfiguration>());
        services.AddSingleton(context.Configuration.GetSection(nameof(TicketingStorageAccountConfiguration)).Get<TicketingStorageAccountConfiguration>());

        services.AddSingleton<ITableStorageService, TableStorageService>();
        services.AddSingleton<IEventProcessingService, EventProcessingService>();

        services.AddSingleton<IOperationProcessor, AppendTickets>();
        services.AddSingleton<IOperationProcessor, BuyTicket>();
        services.AddSingleton<IOperationProcessor, CreateEvent>();
        services.AddSingleton<IOperationProcessor, DeleteEvent>();
        services.AddSingleton<IOperationProcessor, ReturnTicket>();
    })
    .Build();

await host.RunAsync();
