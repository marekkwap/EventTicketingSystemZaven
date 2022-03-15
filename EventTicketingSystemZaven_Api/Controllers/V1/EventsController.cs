using EventTicketingSystemZaven_Api.Models;
using EventTicketingSystemZaven_Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventTicketingSystemZaven_Api.Controllers.V1
{
    [ApiController]
    [Route("v1/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventsService _eventsService;

        public EventsController(
            ILogger<EventsController> logger,
            IEventsService eventsService)
        {
            _logger = logger;
            _eventsService = eventsService;
        }

        [HttpGet(Name = "GetEvents")]
        public async Task<IEnumerable<Event>> GetEvents([FromQuery]IEnumerable<string> ids)
        {
            return await _eventsService.GetEvents(ids);
        }

        [HttpPost(Name = "CreateEvent")]
        public async void CreateEvent([FromBody]Event eventModel)
        {
            await _eventsService.CreateEvent(eventModel);
        }

        [HttpDelete(Name = "DeleteEvent")]
        public async void DeleteEvent([FromQuery]Guid id)
        {
            await _eventsService.DeleteEvent(id);
        }
    }
}