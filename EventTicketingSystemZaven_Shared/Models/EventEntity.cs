using Microsoft.Azure.Cosmos.Table;

namespace EventTicketingSystemZaven_Shared.Models;

/// <summary>
/// Table storage representation of the Event. 
/// PartitionKey => EventId
/// RowKey => "Event"
/// </summary>
public class EventEntity : TableEntity
{
    public string Name { get; set; }

    public int NumberOfTickets { get; set; }
}
