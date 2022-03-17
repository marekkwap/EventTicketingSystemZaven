using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using EventTicketingSystemZaven_Shared.Models;
using Microsoft.Extensions.Logging;

namespace EventTicketingSystemZaven_Api.Services;

public class EventPublisher : IEventPublisher, IDisposable
{
    private readonly EventHubProducerClient _producerClient;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(
        EventHubConfiguration eventHubConfiguration,
        ILogger<EventPublisher> logger)
    {
        _producerClient = new EventHubProducerClient(eventHubConfiguration.TicketingWriteConnectionString, eventHubConfiguration.EventHubName);
        _logger = logger;
    }

    public async Task PublishEvent<T>(EventOperation<T> eventOperation, string partitionId)
    {
        try
        {
            var eventsData = new[] { new EventData(JsonSerializer.Serialize(eventOperation)) };

            await _producerClient.SendAsync(eventsData, new SendEventOptions { PartitionKey = partitionId });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
    }

    public async void Dispose()
    {
        await _producerClient.DisposeAsync();
    }
}
