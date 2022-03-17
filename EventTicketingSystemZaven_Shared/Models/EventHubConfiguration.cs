namespace EventTicketingSystemZaven_Shared.Models;

public class EventHubConfiguration
{
    public string TicketingWriteConnectionString { get; set; }

    public string TicketingListenConnectionString { get; set; }

    public string EventHubName { get; set; }
}
