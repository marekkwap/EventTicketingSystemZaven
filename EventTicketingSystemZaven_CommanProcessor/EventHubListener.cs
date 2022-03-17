using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using EventTicketingSystemZaven_CommandProcessor.Models;
using EventTicketingSystemZaven_CommandProcessor.Services;
using EventTicketingSystemZaven_Shared.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventTicketingSystemZaven_CommandProcessor;

public class EventHubListener : BackgroundService
{
    private readonly ILogger<EventHubListener> _logger;
    private readonly IEventProcessingService _eventProcessingService;
    private readonly EventProcessorClient _eventProcessorClient;

    public EventHubListener(
        ILogger<EventHubListener> logger,
        EventHubConfiguration eventHubConfiguration,
        TicketingStorageAccountConfiguration storageConfiguration,
        IEventProcessingService eventProcessingService)
    {
        _logger = logger;
        _eventProcessingService = eventProcessingService;

        var storageClient = new BlobContainerClient(
            storageConfiguration.ConnectionString,
            storageConfiguration.CheckpointContainerName);
        _eventProcessorClient = new EventProcessorClient(
            storageClient,
            EventHubConsumerClient.DefaultConsumerGroupName,
            eventHubConfiguration.TicketingListenConnectionString,
            eventHubConfiguration.EventHubName);

        _eventProcessorClient.ProcessEventAsync += ProcessEventHandler;
        _eventProcessorClient.ProcessErrorAsync += ProcessErrorHandler;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        return _eventProcessorClient.StartProcessingAsync();
    }

    async Task ProcessEventHandler(ProcessEventArgs eventArgs)
    {
        // Write the body of the event to the console window
        Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));

        await _eventProcessingService.ProcessEvent(eventArgs.Data.EventBody);

        // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
        await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
    }

    static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
    {
        // Write details about the error to the console window
        Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
        Console.WriteLine(eventArgs.Exception.Message);
        return Task.CompletedTask;
    }
}
