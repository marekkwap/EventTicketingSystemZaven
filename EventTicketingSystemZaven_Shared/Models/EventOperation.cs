namespace EventTicketingSystemZaven_Shared.Models;

public class EventOperation<T>
{
    public OperationType OperationType { get; set; }

    public T Payload { get; set; }
}

