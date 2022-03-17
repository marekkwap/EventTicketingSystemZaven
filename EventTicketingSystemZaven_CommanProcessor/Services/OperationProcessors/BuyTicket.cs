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
    internal class BuyTicket : IOperationProcessor
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly ILogger<BuyTicket> _logger;

        public OperationType OperationType => OperationType.BuyTicket;

        public BuyTicket(ITableStorageService tableStorageService, ILogger<BuyTicket> logger)
        {
            _tableStorageService = tableStorageService;
            _logger = logger;
        }

        public async Task ExecuteOperation(JsonElement operationBody)
        {
            try
            {
                var ticketDto = JsonSerializer.Deserialize<EventOperation<TicketDto>>(operationBody).Payload;

                var events = _tableStorageService.QueryEvents<EventEntity>(ticketDto.EventId.ToString(), Constants.EventRowKey);

                if (!events.Any())
                {
                    throw new Exception("Event was not found.");
                }
                var eventEntity = events.First();

                if (eventEntity.NumberOfTickets <= 0)
                {
                    throw new Exception("No tickets avaliable.");
                }

                eventEntity.NumberOfTickets--;

                var ticketEntity = new TicketPurchaseEntity
                {
                    PartitionKey = ticketDto.EventId.ToString(),
                    RowKey = ticketDto.TicketId.ToString(),
                    FristName = ticketDto.FristName,
                    LastName = ticketDto.LastName,
                    Email = ticketDto.Email,
                };

                var transaction = new TableBatchOperation
                {
                    TableOperation.Merge(eventEntity),
                    TableOperation.Insert(ticketEntity)
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
