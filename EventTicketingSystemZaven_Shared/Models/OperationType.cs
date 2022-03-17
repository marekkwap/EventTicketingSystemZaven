using System.Text.Json.Serialization;

namespace EventTicketingSystemZaven_Shared.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OperationType
{
    None = 0,

    CreateEvent,
    DeleteEvent,
    AppendTickets,
    BuyTicket,
    ReturnTicket
}
