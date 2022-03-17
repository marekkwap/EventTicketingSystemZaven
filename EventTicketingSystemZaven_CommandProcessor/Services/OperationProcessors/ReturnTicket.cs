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
    internal class ReturnTicket : IOperationProcessor
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly ILogger<BuyTicket> _logger;

        public OperationType OperationType => OperationType.ReturnTicket;

        public ReturnTicket(ITableStorageService tableStorageService, ILogger<BuyTicket> logger)
        {
            _tableStorageService = tableStorageService;
            _logger = logger;
        }

        public async Task ExecuteOperation(JsonElement operationBody)
        {
            var ticketDto = JsonSerializer.Deserialize<EventOperation<TicketDto>>(operationBody).Payload;

            try
            {
                var eventQueryResult = _tableStorageService.QueryEvents<EventEntity>(ticketDto.EventId.ToString(), Constants.EventRowKey);

                if (!eventQueryResult.Any())
                {
                    throw new Exception("Event was not found.");
                }

                var ticketQueryResult = _tableStorageService.QueryEvents(ticketDto.EventId.ToString(), ticketDto.TicketId.ToString());

                if (!ticketQueryResult.Any())
                {
                    throw new Exception("Ticket was not found.");
                }

                var eventEntity = eventQueryResult.First();
                eventEntity.NumberOfTickets++;

                var transaction = new TableBatchOperation
                {
                    TableOperation.Delete(ticketQueryResult.First()),
                    TableOperation.Merge(eventEntity)
                };

                await _tableStorageService.EventsTable.ExecuteBatchAsync(transaction).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
