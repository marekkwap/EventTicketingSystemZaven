using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using EventTicketingSystemZaven_Api.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_Api.Services
{
    public class EventPublisher : IEventPublisher, IDisposable
    {
        private readonly EventHubProducerClient _producerClient;
        private readonly ILogger<EventPublisher> _logger;

        public EventPublisher(
            EventHubConfiguration eventHubConfiguration,
            ILogger<EventPublisher> logger)
        {
            _producerClient = new EventHubProducerClient(eventHubConfiguration.ConnectionString, eventHubConfiguration.EventHubName);
            _logger = logger;
        }

        public async Task PublishEvent<T>(EventOperation<T> eventOperation)
        {
            try
            {
                var eventsData = new[] { new EventData(JsonSerializer.Serialize(eventOperation)) };

                await _producerClient.SendAsync(eventsData, new SendEventOptions { PartitionKey = eventOperation.EventId.ToString() });
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
}
