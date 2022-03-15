using EventTicketingSystemZaven_Api.Models;
using EventTicketingSystemZaven_Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EventTicketingSystemZaven_Api.Controllers.V1
{

    [ApiController]
    [Route("v1/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ILogger<TicketsController> _logger;
        private readonly ITicketsService _ticketsService;

        public TicketsController(
            ILogger<TicketsController> logger,
            ITicketsService ticketsService)
        {
            _logger = logger;
            _ticketsService = ticketsService;
        }

        [HttpPost(Name = "BuyTicket")]
        public async void BuyTicket([FromBody]TicketPurchase ticketPurchase)
        {
            await _ticketsService.BuyTicket(ticketPurchase);
        }

        [HttpDelete(Name = "ReturnTicket")]
        public async void ReturnTicket([FromBody] TicketPurchase ticketPurchase)
        {
            await _ticketsService.ReturnTicket(ticketPurchase);
        }
    }
}
