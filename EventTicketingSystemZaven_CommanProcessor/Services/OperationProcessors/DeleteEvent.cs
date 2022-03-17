using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EventTicketingSystemZaven_CommanProcessor.Services.Interfaces;
using EventTicketingSystemZaven_Shared.Models;
using EventTicketingSystemZaven_Shared.Services;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace EventTicketingSystemZaven_CommanProcessor.Services.OperationProcessors
{
    internal class DeleteEvent : IOperationProcessor
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly ILogger<CreateEvent> _logger;

        public OperationType OperationType => OperationType.DeleteEvent;

        public DeleteEvent(ITableStorageService tableStorageService, ILogger<CreateEvent> logger)
        {
            _tableStorageService = tableStorageService;
            _logger = logger;
        }

        public async Task ExecuteOperation(JsonElement operationBody)
        {
            try
            {
                var eventDto = JsonSerializer.Deserialize<EventOperation<EventDto>>(operationBody).Payload;

                var queryResult = _tableStorageService.QueryEvents(eventDto.EventId.ToString());

                if (!queryResult.Any())
                {
                    throw new Exception("Event was not found.");
                }

                var transaction = new TableBatchOperation();
                foreach (var result in queryResult)
                {
                    transaction.Delete(result);
                }

                await _tableStorageService.EventsTable.ExecuteBatchAsync(transaction).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
