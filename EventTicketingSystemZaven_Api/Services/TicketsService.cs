using EventTicketingSystemZaven_Api.Models;
using System;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_Api.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly IEventPublisher _eventPublisher;

        public TicketsService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task BuyTicket(TicketPurchase ticketPurchase)
        {
            var eventOperation = new EventOperation<TicketPurchase>
            {
                EventId = ticketPurchase.EventId,
                OperationType = OperationType.BuyTicket,
                Payload = ticketPurchase
            };

            await _eventPublisher.PublishEvent(eventOperation);
        }

        public async Task ReturnTicket(TicketPurchase eventModel)
        {
            var eventOperation = new EventOperation<TicketPurchase>
            {
                EventId = eventModel.EventId,
                OperationType = OperationType.BuyTicket,
                Payload = eventModel
            };

            await _eventPublisher.PublishEvent(eventOperation);
        }
    }
}
