using System;

namespace EventTicketingSystemZaven_Api.Models
{
    public class TicketPurchase
    {
        public Guid EventId { get; set; }

        public Guid TicketId { get; set; }

        public string FristName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}