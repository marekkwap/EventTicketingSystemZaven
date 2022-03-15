using EventTicketingSystemZaven_Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_Api.Services
{
    public interface IEventsService
    {
        Task<IEnumerable<Event>> GetEvents(IEnumerable<string> ids);
        Task CreateEvent(Event eventModel);
        Task DeleteEvent(Guid id);
    }
}