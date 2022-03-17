using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventTicketingSystemZaven_Api.Models;
using EventTicketingSystemZaven_Api.Services;
using EventTicketingSystemZaven_Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventTicketingSystemZaven_Shared.Controllers.V1;

[ApiController]
[Route("v1/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IEventsService _eventsService;

    public EventsController(ILogger<EventsController> logger, IEventsService eventsService)
    {
        _logger = logger;
        _eventsService = eventsService;
    }

    [HttpGet]
    public IEnumerable<EventDto> GetEvents([FromQuery] IEnumerable<string> ids)
    {
        _logger.LogDebug($"Recieved GetEvents request for ids: {Request}");

        return _eventsService.GetEvents(ids);
    }

    [HttpPost]
    public async void CreateEvent([FromBody] CreateEventDto eventModel)
    {
        _logger.LogDebug($"Recieved BuyTicketRequest: {Request.Body}");

        await _eventsService.CreateEvent(eventModel).ConfigureAwait(false);
    }


    [Route("{eventId}/appendTickets")]
    [HttpPost]
    public async void AppendTickets([FromRoute]string eventId, [FromQuery] int numberOfTickets)
    {
        _logger.LogDebug($"Recieved BuyTicketRequest: {Request.Body}");

        await _eventsService.AppendTickets(eventId, numberOfTickets).ConfigureAwait(false);
    }

    [HttpDelete]
    public async void DeleteEvent([FromQuery]string id)
    {
        _logger.LogDebug($"Recieved BuyTicketRequest: {Request.Body}");

        await _eventsService.DeleteEvent(id).ConfigureAwait(false);
    }
}
