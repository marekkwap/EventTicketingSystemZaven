using System;
using System.Collections.Generic;
using EventTicketingSystemZaven_Shared.Models;
using Microsoft.Azure.Cosmos.Table;

namespace EventTicketingSystemZaven_Shared.Services;

public interface ITableStorageService
{
    public CloudTable EventsTable { get; }

    IEnumerable<TableEntity> QueryEvents(string partitionKey, string rowKey = null);

    IEnumerable<T> QueryEvents<T>(string partitionKey, string rowKey = null) where T : ITableEntity, new();

    IEnumerable<EventEntity> QueryEvents(IEnumerable<string> partitionKeys);

    IEnumerable<TicketPurchaseEntity> GetTickets(Guid eventId);
}
