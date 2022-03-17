using System;

namespace EventTicketingSystemZaven_Shared.Models;

public class TicketDto
{
    public string TicketId { get; set; }

    public string EventId { get; set; }

    public string FristName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }
}
