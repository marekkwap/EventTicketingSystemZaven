using EventTicketingSystemZaven_Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_Api.Services
{
    public class EventsService : IEventsService
    {
        private readonly IEventPublisher _eventPublisher;

        public EventsService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task CreateEvent(Event eventModel)
        {
            var operation = new EventOperation<Event>
            {
                EventId = Guid.NewGuid(),
                OperationType = OperationType.CreateEvent,
                Payload = eventModel
            };

            await _eventPublisher.PublishEvent(operation);
        }

        public async Task DeleteEvent(Guid eventId)
        {
            var operation = new EventOperation<Event>
            {
                EventId = eventId,
                OperationType = OperationType.DeleteEvent
            };

            await _eventPublisher.PublishEvent(operation);
        }

        public async Task AppendTickets(string id, int numberOfTickets)
        {
            var operation = new EventOperation<Event>
            {
                EventId = Guid.NewGuid(),
                OperationType = OperationType.AppendTickets,
                Payload = { NumberOfTickets = numberOfTickets }
            };

            await _eventPublisher.PublishEvent(operation);
        }

        public async Task<IEnumerable<Event>> GetEvents(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }
    }
}
