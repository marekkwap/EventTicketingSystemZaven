using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventTicketingSystemZaven_Api.Models;
using EventTicketingSystemZaven_Api.Services;
using EventTicketingSystemZaven_Shared.Models;

namespace EventTicketingSystemZaven_Shared.Services
{
    public class EventsService : IEventsService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ITableStorageService _tableStorageService;

        public EventsService(IEventPublisher eventPublisher, ITableStorageService tableStorageService)
        {
            _eventPublisher = eventPublisher;
            _tableStorageService = tableStorageService;
        }

        public async Task CreateEvent(CreateEventDto eventModel)
        {
            var payload = new EventDto
            {
                EventId = Guid.NewGuid().ToString(),
                Name = eventModel.Name,
                NumberOfTickets = eventModel.NumberOfTickets,
            };

            var operation = new EventOperation<EventDto>
            {
                OperationType = OperationType.CreateEvent,
                Payload = payload
            };

            await _eventPublisher.PublishEvent(operation, payload.EventId.ToString()).ConfigureAwait(false);
        }

        public async Task DeleteEvent(string eventId)
        {
            var operation = new EventOperation<EventDto>
            {
                OperationType = OperationType.DeleteEvent,
                Payload = new EventDto { EventId = eventId }
            };

            await _eventPublisher.PublishEvent(operation, eventId.ToString()).ConfigureAwait(false);
        }

        public async Task AppendTickets(string eventId, int numberOfTickets)
        {
            var operation = new EventOperation<EventDto>
            {
                OperationType = OperationType.AppendTickets,
                Payload = new EventDto { NumberOfTickets = numberOfTickets, EventId = eventId }
            };

            await _eventPublisher.PublishEvent(operation, eventId.ToString()).ConfigureAwait(false);
        }

        public IEnumerable<EventDto> GetEvents(IEnumerable<string> ids)
        {
            var tableEntities = _tableStorageService.QueryEvents(ids);

            return tableEntities.Select(entity => new EventDto
            {
                EventId = entity.PartitionKey,
                Name = entity.Name,
                NumberOfTickets = entity.NumberOfTickets,
            });
        }
    }
}
