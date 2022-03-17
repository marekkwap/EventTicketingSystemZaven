using System;
using Azure.Identity;
using EventTicketingSystemZaven_CommanProcessor;
using EventTicketingSystemZaven_CommanProcessor.Models;
using EventTicketingSystemZaven_CommanProcessor.Services;
using EventTicketingSystemZaven_CommanProcessor.Services.Interfaces;
using EventTicketingSystemZaven_CommanProcessor.Services.OperationProcessors;
using EventTicketingSystemZaven_Shared.Models;
using EventTicketingSystemZaven_Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddAzureKeyVault(
            new Uri("https://ticketingkv.vault.azure.net/"),
            new DefaultAzureCredential());
    })
    .ConfigureServices((context, services) =>
    {
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
