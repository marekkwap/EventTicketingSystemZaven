using Microsoft.Azure.Cosmos.Table;
using System;

namespace EventTicketingSystemZaven_CommanProcessor.Models
{
    public class Event : TableEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TotalTickets { get; set; }

        public int AvaliableTickets { get; set; }
    }
}