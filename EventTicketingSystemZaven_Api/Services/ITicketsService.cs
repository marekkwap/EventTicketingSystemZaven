using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventTicketingSystemZaven_Api.Models;
using EventTicketingSystemZaven_Shared.Models;

namespace EventTicketingSystemZaven_Api.Services;

public interface ITicketsService
{
    IEnumerable<TicketDto> GetTicketsList(Guid eventId);

    Task BuyTicket(BuyTicketDto ticketPurchase);

    Task ReturnTicket(ReturnTicketDto eventModel);
}
