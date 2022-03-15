using Azure.Identity;
using EventTicketingSystemZaven_CommanProcessor;
using EventTicketingSystemZaven_CommanProcessor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
    })
    .Build();

await host.RunAsync();
