using System;
using System.Text.Json;
using System.Threading.Tasks;
using EventTicketingSystemZaven_CommandProcessor.Services.Interfaces;
using EventTicketingSystemZaven_Shared;
using EventTicketingSystemZaven_Shared.Models;
using EventTicketingSystemZaven_Shared.Services;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace EventTicketingSystemZaven_CommandProcessor.Services.OperationProcessors
{
    internal class CreateEvent : IOperationProcessor
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly ILogger<CreateEvent> _logger;

        public OperationType OperationType => OperationType.CreateEvent;

        public CreateEvent(ITableStorageService tableStorageService, ILogger<CreateEvent> logger)
        {
            _tableStorageService = tableStorageService;
            _logger = logger;
        }

        public async Task ExecuteOperation(JsonElement operationBody)
        {
            try
            {
                var eventDto = JsonSerializer.Deserialize<EventOperation<EventDto>>(operationBody).Payload;

                var eventEntity = new EventEntity
                {
                    PartitionKey = eventDto.EventId.ToString(),
                    RowKey = Constants.EventRowKey,
                    Name = eventDto.Name,
                    NumberOfTickets = eventDto.NumberOfTickets,
                };

                await _tableStorageService.EventsTable.ExecuteAsync(TableOperation.Insert(eventEntity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
