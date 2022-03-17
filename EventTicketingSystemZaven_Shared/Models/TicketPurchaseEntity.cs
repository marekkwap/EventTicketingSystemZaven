using Microsoft.Azure.Cosmos.Table;

namespace EventTicketingSystemZaven_Shared.Models;

/// <summary>
/// Table storage representation of the Event. 
/// PartitionKey => EventId
/// RowKey => TicketId
/// </summary>
public class TicketPurchaseEntity : TableEntity
{
    public string FristName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }
}
