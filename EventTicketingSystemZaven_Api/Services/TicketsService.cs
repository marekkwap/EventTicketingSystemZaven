using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventTicketingSystemZaven_Api.Models;
using EventTicketingSystemZaven_Shared.Models;
using EventTicketingSystemZaven_Shared.Services;

namespace EventTicketingSystemZaven_Api.Services;

public class TicketsService : ITicketsService
{
    private readonly IEventPublisher _eventPublisher;
    private readonly ITableStorageService _tableStorageService;

    public TicketsService(IEventPublisher eventPublisher, ITableStorageService tableStorageService)
    {
        _eventPublisher = eventPublisher;
        _tableStorageService = tableStorageService;
    }

    public IEnumerable<TicketDto> GetTicketsList(Guid eventId)
    {
        var ticketsEntities = _tableStorageService.GetTickets(eventId);

        return ticketsEntities.Select(entity => new TicketDto
        {
            TicketId = entity.RowKey,
            EventId = entity.PartitionKey,
            FristName = entity.FristName,
            LastName = entity.LastName,
            Email = entity.Email,
        });
    }

    public async Task BuyTicket(BuyTicketDto ticketDto)
    {
        var ticketPurchase = new TicketDto
        {
            EventId = ticketDto.EventId,
            TicketId = Guid.NewGuid().ToString(),
            FristName = ticketDto.FristName,
            LastName = ticketDto.LastName,
            Email = ticketDto.Email,
        };

        var eventOperation = new EventOperation<TicketDto>
        {
            OperationType = OperationType.BuyTicket,
            Payload = ticketPurchase
        };

        await _eventPublisher.PublishEvent(eventOperation, ticketPurchase.EventId.ToString());
    }

    public async Task ReturnTicket(ReturnTicketDto ticketDto)
    {
        var ticketPurchase = new TicketDto
        {
            EventId = ticketDto.EventId,
            TicketId = ticketDto.TicketId
        };

        var eventOperation = new EventOperation<TicketDto>
        {
            OperationType = OperationType.ReturnTicket,
            Payload = ticketPurchase
        };

        await _eventPublisher.PublishEvent(eventOperation, ticketPurchase.EventId.ToString());
    }
}
