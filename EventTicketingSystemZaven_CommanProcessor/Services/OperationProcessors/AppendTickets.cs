using System;
using System.Linq;
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
    internal class AppendTickets : IOperationProcessor
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly ILogger<AppendTickets> _logger;

        public OperationType OperationType => OperationType.AppendTickets;

        public AppendTickets(ITableStorageService tableStorageService, ILogger<AppendTickets> logger)
        {
            _tableStorageService = tableStorageService;
            _logger = logger;
        }

        public async Task ExecuteOperation(JsonElement operationBody)
        {
            try
            {
                var eventDto = JsonSerializer.Deserialize<EventOperation<EventDto>>(operationBody).Payload;

                var queryResult = _tableStorageService.QueryEvents<EventEntity>(eventDto.EventId.ToString(), Constants.EventRowKey);

                if (!queryResult.Any())
                {
                    throw new Exception("Event was not found.");
                }

                var eventEntity = queryResult.First();
                eventEntity.NumberOfTickets += eventDto.NumberOfTickets;

                TableResult result = await _tableStorageService.EventsTable.ExecuteAsync(TableOperation.Merge(eventEntity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
