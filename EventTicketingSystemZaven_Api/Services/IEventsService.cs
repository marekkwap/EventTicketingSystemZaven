using System.Collections.Generic;
using System.Threading.Tasks;
using EventTicketingSystemZaven_Api.Models;
using EventTicketingSystemZaven_Shared.Models;

namespace EventTicketingSystemZaven_Api.Services;

public interface IEventsService
{
    IEnumerable<EventDto> GetEvents(IEnumerable<string> ids);
    Task CreateEvent(CreateEventDto eventModel);
    Task DeleteEvent(string id);
    Task AppendTickets(string eventId, int numberOfTickets);
}
