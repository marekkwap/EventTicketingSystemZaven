namespace EventTicketingSystemZaven_Api.Models
{
    public enum OperationType
    {
        None = 0,

        CreateEvent,
        DeleteEvent,
        AppendTickets,
        BuyTicket,
        ReturnTicket
    }
}