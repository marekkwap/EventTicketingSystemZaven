using System;
using System.Collections.Generic;
using EventTicketingSystemZaven_Api.Models;
using EventTicketingSystemZaven_Api.Services;
using EventTicketingSystemZaven_Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventTicketingSystemZaven_Api.Controllers.V1;

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

    [HttpGet]
    [Route("{eventId}")]
    public IEnumerable<TicketDto> GeTicketsList([FromRoute]Guid eventId)
    {
        _logger.LogDebug($"Recieved BuyTicketRequest: {Request.Body}");

        return _ticketsService.GetTicketsList(eventId);
    }

    [HttpPost]
    public async void BuyTicket([FromBody] BuyTicketDto ticketPurchase)
    {
        _logger.LogDebug($"Recieved BuyTicketRequest: {Request.Body}");

        await _ticketsService.BuyTicket(ticketPurchase).ConfigureAwait(false);
    }

    [HttpDelete]
    public async void ReturnTicket([FromBody] ReturnTicketDto ticketPurchase)
    {
        _logger.LogDebug($"Recieved ReturnTicketRequest: {Request.Body}");

        await _ticketsService.ReturnTicket(ticketPurchase).ConfigureAwait(false);
    }
}
