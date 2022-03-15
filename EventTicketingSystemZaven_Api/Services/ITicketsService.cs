using EventTicketingSystemZaven_Api.Models;
using System;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_Api.Services
{
    public interface ITicketsService
    {
        Task BuyTicket(TicketPurchase ticketPurchase);
        Task ReturnTicket(TicketPurchase eventModel);
    }
}