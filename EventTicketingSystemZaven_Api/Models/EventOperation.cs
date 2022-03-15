using System;

namespace EventTicketingSystemZaven_Api.Models
{
    public class EventOperation<T>
    {
        public Guid EventId { get; set; }

        public OperationType OperationType { get; set; }

        public T Payload { get; set; }
    }
}
